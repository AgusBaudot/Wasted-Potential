using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IUpdatable
{
    [SerializeField] private float speed = 5f;

    private EnemyBase _target;
    private Tower _source;
    private int _baseDamage;

    public void Init(EnemyBase target, Tower source, int dmg)
    {
        _target = target;
        _source = source;
        _baseDamage = dmg;
        ServiceLocator.Get<UpdateManager>().Register(this);
        
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
        ServiceLocator.Get<UpdateManager>().Unregister(this);
        ServiceLocator.Get<ProjectilePool>().Release(this);
    }

    public void Tick(float deltaTime)
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.WorldPosition, Time.deltaTime * speed); 
    }
}