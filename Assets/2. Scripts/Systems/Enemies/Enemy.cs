using UnityEngine;
using System;

public abstract class EnemyBase : MonoBehaviour, IUpdatable, IPoolable, ITargetable
{
    //Enemy has to fire event when end is reached or dies.
    public event Action<EnemyBase> OnRemoved;
    
    public EnemyData Data { get; private set; }
    
    public Health Health { get; private set; }

    public Vector3 WorldPosition => transform.position;
    public bool IsAlive => Health.Current > 0;
    
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
    }

    public virtual void Tick(float deltaTime)
    {
        if (currentTile == null || currentTile.Type == GridTileType.Goal)
        {
            ReachedEnd();
            return;
        }

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

        //Since speed represents seconds per tile (less speed number actually means faster), divide instead of multiply.
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, deltaTime / Data.moveSpeed);
    }

    public virtual void ApplyDamage(int amount, GameObject source)
    {
        Health.TakeDamage(amount);
        //Handle hit reactions, damage source, etc.
    }

    public void ApplyDot(int dmgPerSec, float duration)
    {
        //Every 1 second, call ApplyDamage(dmgPerSec).
        
    }

    public void ApplySlow(float factor, float duration)
    {
        //Modify speed for duration.
    }

    public virtual void Die()
    {
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
        transform.position = _spawnPosition;
        gameObject.SetActive(false);
    }

    public void ReturnToPool() => _originFactory?.Release(this);
}