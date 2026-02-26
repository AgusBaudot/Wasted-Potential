using System;
using UnityEngine;
using VContainer;

public class Projectile : MonoBehaviour, IUpdatable
{
    [SerializeField] private float speed = 5f;

    private EnemyBase _target;
    private Tower _source;
    private int _baseDamage;
    private IUpdateManager _updateManager;
    private IProjectilePool _pool;

    [Inject]
    public void Construct(IUpdateManager updateManager)
    {
        _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
    }

    public void Init(EnemyBase target, Tower source, int dmg, IProjectilePool pool)
    {
        _target = target;
        _source = source;
        _baseDamage = dmg;
        _pool = pool;
        
        _updateManager.Register(this);
        transform.position = source.gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyBase>();
        if (enemy == null) return;

        _source.NotifyHitEnemy(enemy, this);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        _updateManager.Unregister(this);
        _pool.Release(this);
    }

    public void Tick(float deltaTime)
    {
        if (!_target.isActiveAndEnabled) 
            ReturnToPool();
        transform.position = Vector2.MoveTowards(transform.position, _target.WorldPosition, Time.deltaTime * speed); 
    }
}