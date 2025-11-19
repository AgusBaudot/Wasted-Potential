using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tower : MonoBehaviour, IUpdatable
{
    public event Action<Tower> OnMouseEnterTower;
    public event Action<Tower> OnMouseExitTower;

    public Vector2Int GridPosistion { get; private set; }
    public CardData Data { get; private set; }

    private ITargetingStrategy _strategy;
    private IAttackBehavior _attackBehavior;

    private bool _canShoot = true;
    private float _fireTimer;
    private UpdateManager _updateManager;
    private EnemyManager _enemyManager;
    private EnemyBase _currentTarget;
    private TowerManager _towerManager;


    //Called by factory/command immediately after creation.
    public void Initialize(CardData data, Vector2Int gridPosition)
    {
        Data = data;
        GridPosistion = gridPosition;

        _updateManager ??= ServiceLocator.Get<UpdateManager>();
        _enemyManager ??= ServiceLocator.Get<EnemyManager>();
        _towerManager ??= ServiceLocator.Get<TowerManager>();

        _updateManager.Register(this);
        _towerManager.RegisterTower(this);

        _strategy = new PickClosest();
        _attackBehavior = data.usesProjectile
            ? new ProjectileAttack()
            : new InstantAttack();

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

        Data.ability?.OnTick(this, deltaTime);
    }

    private void TryAttack()
    {
        if (_enemyManager == null)
            _enemyManager = ServiceLocator.Get<EnemyManager>();
        if (_strategy == null)
            _strategy = new PickClosest();

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

        _attackBehavior.Execute(this, target);

        //// Deal damage
        //if (Data.ability != null)
        //{
        //    //Make OnFire spawn projectile and notify when OnEnemyHit occurs.
        //    Data.ability.OnFire(this, target.gameObject);

        //    //var proj = ServiceLocator.Get<ProjectilePool>().Spawn(customProjectilePrefab, transform.position);
        //    var proj = ServiceLocator.Get<ProjectilePool>().Spawn(transform.position);
        //    proj.Init(target, this, Data.damage);
        //}

        // Begin cooldown
        _canShoot = false;
        _fireTimer = 0f;
    }

    public void NotifyHitEnemy(EnemyBase enemy, Projectile projectile)
    {
        var handled = Data.ability.OnEnemyHit(this, enemy);

        //Analyze return of OnEnemyHit to know if I need to call ApplyDamage from here.
        if (!handled)
        {
            Debug.LogWarning("Ability did not handle OnEnemyHit, applying damage directly.");
            if (enemy is ITargetable t) t.ApplyDamage(Data.damage, gameObject);
        }
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