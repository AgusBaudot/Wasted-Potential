using System;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyStatus
{
    None,
    Dot,
    Slow,
    Stun
}

public abstract class EnemyBase : MonoBehaviour, IUpdatable, IPoolable, ITargetable
{
    //Enemy has to fire event when end is reached or dies.
    public event Action<EnemyBase> OnRemoved;
    
    public EnemyData Data { get; private set; }
    
    public Health Health { get; private set; }

    public Vector3 WorldPosition => transform.position;
    public bool IsAlive => Health.Current > 0;
    public EnemyStatusManager StatusManager => _statusManager;
    
    protected EnemyStatusManager _statusManager;
    protected float _baseMoveSpeed => Data.moveSpeed;
    
    protected GridManager _grid;
    protected GridTile currentTile;
    protected Vector3 _spawnPosition;
    protected Vector3 _targetPos;
    protected UpdateManager _updateManager;
    protected IEnemyFactory _originFactory;


    public virtual void Initialize(EnemyData data, Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        Data = data;
        _spawnPosition = spawnPosition;
        _originFactory = originFactory;
        transform.position = _spawnPosition;

        _grid = ServiceLocator.Get<GridManager>();
        currentTile = _grid.GetTile(_grid.WorldToGrid(spawnPosition));
        _targetPos = _grid.GetTile(currentTile.Next).Center;

        _updateManager ??= ServiceLocator.Get<UpdateManager>();
        _updateManager.Register(this);
        
        if (Health == null)
        {
            Health = new Health(Data.maxHealth);
            Health.OnDeath += Die;
        }
        Health.Reset();
        ServiceLocator.Get<HealthBarManager>().Register(Health, transform);

        _statusManager = new EnemyStatusManager(this);
    }

    public virtual void Tick(float deltaTime)
    {
        //1. Tick the manager since it handles DoT, expirations and reactions.
        _statusManager.Tick(deltaTime);
        
        //2. Check stun via manager.
        if (_statusManager.IsStunned())
        {
            //Visual feedback for stunned could go here.
            return;
        }

        #region Tile logic
        
        //Navigation logic.
        if (currentTile == null || currentTile.Type == GridTileType.Goal)
        {
            ReachedEnd();
            return;
        }
        
        //Tile switching logic.
        if (Vector3.Distance(transform.position, _targetPos) < 0.1f)
        {
            transform.position = _targetPos;
            var nextTile = _grid.GetTile(currentTile.Next);

            if (nextTile == null)
            {
                ReachedEnd();
                return;
            }

            currentTile = nextTile; // Update tile based on logic, not position
            _targetPos = nextTile.Center;
        }

        #endregion
        
        //3. Get speed via manager.
        float currentSpeedMod = _statusManager.GetSpeedMultiplier();
        
        //Safety clamp to prevent negative speed or division by zero errors.
        float effectiveSpeed = _baseMoveSpeed / Mathf.Clamp(currentSpeedMod, 0.01f, 5);

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, deltaTime / effectiveSpeed);
    }

    protected void ClearStatus()
    {
        _statusManager?.ClearAll();
    }

    //API needed by StatusEffects that do Instant Damage.
    public virtual void ApplyDamage(float amount, GameObject source)
    {
        Health.TakeDamage(amount);
    }

    public void ApplyStatus(StatusEffect def) => _statusManager.Apply(def);

    public virtual void Die()
    {
        ClearStatus();
        ServiceLocator.Get<HealthBarManager>().Unregister(Health);
        OnRemoved?.Invoke(this);
    }

    protected virtual void ReachedEnd()
    {
        ServiceLocator.Get<HealthBarManager>().Unregister(Health);
        ServiceLocator.Get<PlayerHealthManager>().ApplyDamage(Data.damageOnReach);
        OnRemoved?.Invoke(this);
    }

    public virtual void Reset()
    {
        _updateManager.Unregister(this);
        ClearStatus();
        transform.position = _spawnPosition;
        gameObject.SetActive(false);
    }

    public void ReturnToPool() => _originFactory?.Release(this);
}