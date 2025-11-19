using System;
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
    
    protected EnemyStatus _status = EnemyStatus.None;
    protected float _statusTimer = 0f;
    
    //DOT specific.
    protected int _dotDmgPerSec = 0;
    protected float _dotTickAcc = 0f;
    
    //Slow specific.
    //Factor: 1 = normal, 0.5 = half speed (slower).
    protected float _moveSpeedMultiplier = 1f;
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
    }

    public virtual void Tick(float deltaTime)
    {
        if (currentTile == null || currentTile.Type == GridTileType.Goal)
        {
            ReachedEnd();
            return;
        }
        
        ProcessStatus(deltaTime);

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
        float effectiveSecondsPerTile = _baseMoveSpeed / Mathf.Clamp(_moveSpeedMultiplier, 0.0001f, 1000f);
        if (_status != EnemyStatus.Stun)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, deltaTime / effectiveSecondsPerTile);
        }
    }

    protected void ProcessStatus(float deltaTime)
    {
        if (_status == EnemyStatus.None) return;
        
        _statusTimer -= deltaTime;
        if (_status == EnemyStatus.Dot)
        {
            //Accumulate fractional seconds and apply damage every full second.
            _dotTickAcc += deltaTime;
            while (_dotTickAcc >= 1f && _statusTimer > 0f)
            {
                _dotTickAcc -= 1f;
                ApplyDamage(_dotDmgPerSec, null);
                
                //If enemy died form the DOT, stop processing
                if (!IsAlive)
                {
                    //Status will be cleaned in Die()
                    return;
                }
            }
        }

        if (_statusTimer <= 0f)
            ClearStatus();
    }

    protected void ClearStatus()
    {
        _status = EnemyStatus.None;
        _statusTimer = 0f;
        _dotDmgPerSec = 0;
        _dotTickAcc = 0;
        _moveSpeedMultiplier = 1;
    }

    public virtual void ApplyDamage(int amount, GameObject source)
    {
        Health.TakeDamage(amount);
        //Handle hit reactions, damage source, etc.
    }

    //Public API used by tower abilities
    public void ApplyDot(int dmgPerSec, float duration)
    {
        //Replace current status with DOT.
        _status = EnemyStatus.Dot;
        _statusTimer = duration;
        _dotDmgPerSec = Mathf.Max(0, dmgPerSec);
        _dotTickAcc = 0; //Start tick fresh.
        //Optionally: trigger VFX or status icon.
    }

    //Public API used by tower abilities
    public void ApplySlow(float factor, float duration)
    {
        //Replace current status ith Slow.
        _status = EnemyStatus.Slow;
        _statusTimer = duration;
        _moveSpeedMultiplier = Mathf.Clamp(factor, 0.0001f, 10f);
        //Reset DOT accumulator so old DOT doesn't apply later (statuses are replaced)
        _dotTickAcc = 0;
        _dotDmgPerSec = 0;
    }

    //Public API used by tower abilities
    public void ApplyStun(float time)
    {
        _statusTimer = time;
        _status = EnemyStatus.Stun;
    }

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