using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IUpdatable
{
    public Vector2Int GridPosistion { get; private set; }
    public CardData Data { get; private set; }

    private ITargetingStrategy _strategy;
    private bool _canShoot = true;
    private float _fireTimer;
    private UpdateManager _updateManager;
    private EnemyManager _enemyManager;
    private Enemy _currentTarget;


    //Called by factory/command immediately after creation.
    public void Initialize(CardData data, Vector2Int gridPosition)
    {
        Data = data;
        GridPosistion = gridPosition;

        _updateManager ??= ServiceLocator.Get<UpdateManager>();
        _enemyManager ??= ServiceLocator.Get<EnemyManager>();

        _updateManager.Register(this);

        _strategy = new PickClosest();

        _canShoot = true;
        _fireTimer = 0;
    }

    public void OnDestroy()
    {
        _updateManager.Unregister(this);
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
    }

    private void TryAttack()
    {
        if (_enemyManager == null)
            _enemyManager = ServiceLocator.Get<EnemyManager>();
        if (_strategy == null)
            _strategy = new PickClosest();

        // Get valid candidates
        List<Enemy> candidates = _enemyManager.GetEnemiesInRange(transform.position, Data.range);
        if (candidates == null || candidates.Count == 0)
            return;

        candidates.RemoveAll(e => e == null || !e.IsAlive);
        if (candidates.Count == 0)
            return;

        // Select target
        Enemy target = _strategy.SelectTarget(candidates, this);
        if (target == null || !target.IsAlive)
            return;

        // Deal damage
        if (target is ITargetable targetable)
            targetable.ApplyDamage(Data.damage, gameObject);

        // Begin cooldown
        _canShoot = false;
        _fireTimer = 0f;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Data.range);
    }
}