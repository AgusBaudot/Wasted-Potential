using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class Tower : MonoBehaviour, IUpdatable
{
    public event Action<Tower> OnMouseEnterTower;
    public event Action<Tower> OnMouseExitTower;

    public Vector2Int GridPosistion { get; private set; }
    public CardData Data { get; private set; }
    public IObjectResolver Container => _container;

    private ITargetingStrategy _strategy;

    private bool _canShoot = true;
    private float _fireTimer;
    private IUpdateManager _updateManager;
    private IEnemyQuery _enemyManager;
    private ITowerRegistry _towerManager;
    private EnemyBase _currentTarget;
    private IObjectResolver _container;

    [Inject]
    public void Construct(IUpdateManager updateManager, IEnemyQuery enemyManager, ITowerRegistry towerManager, IObjectResolver container)
    {
        _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
        _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
        _towerManager = towerManager ?? throw new ArgumentNullException(nameof(towerManager));
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }


    //Called by factory/command immediately after creation.
    public void Initialize(CardData data, Vector2Int gridPosition)
    {
        Data = data;
        GridPosistion = gridPosition;

        _updateManager.Register(this);
        _towerManager.RegisterTower(this);

        _strategy = new PickClosest();

        _canShoot = true;
        _fireTimer = 0;
        Data.ability?.OnPlaced(this);
    }

    public void OnDestroy()
    {
        _updateManager.Unregister(this);
        _towerManager.UnregisterTower(this);
    }

    public void Tick(float deltaTime)
    {
        Data.ability?.OnTick(this, deltaTime);

        if (_canShoot)
        {
            TryAttack();
            return;
        }

        _fireTimer += deltaTime;
        if (_fireTimer >= Data.fireRate)
        {
            _canShoot = true;
            _fireTimer = 0;
            TryAttack();
        }
    }

    private void TryAttack()
    {
        // Get valid candidates
        List<EnemyBase> candidates = _enemyManager.GetEnemiesInRange(transform.position, Data.range);
        if (candidates == null || candidates.Count == 0)
            return;

        candidates.RemoveAll(e => e == null || !e.IsAlive);
        if (candidates.Count == 0)
            return;

        // Select target
        EnemyBase target = _strategy.SelectTarget(candidates, this);
        if (target == null || !target.IsAlive)
            return;

        if (TryGetComponent<Animator>(out var anim))
        {
            if (anim.parameters.Any(param => param.name == "Attack"))
            {
                anim.SetTrigger("Attack");
            }
        }
        Data.ability.Fire(this, target);

        // Begin cooldown
        _canShoot = false;
        _fireTimer = 0f;
    }

    public void NotifyHitEnemy(EnemyBase enemy, Projectile projectile)
    {
        Data.ability.OnEnemyHit(this, enemy);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 size = new Vector3(Data.range * 2, Data.range * 2);
        Gizmos.DrawWireCube(transform.position, size);
    }
    
    private void OnMouseEnter()
    {
        OnMouseEnterTower?.Invoke(this);
    }

    private void OnMouseExit()
    {
        OnMouseExitTower?.Invoke(this);
    }
}