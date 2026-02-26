using UnityEngine;

[CreateAssetMenu(menuName = "TD/Attacks/Projectile attack")]
public class ProjectileAttack : TowerAbility
{
    public Projectile projectilePrefab;

    public override void Fire(Tower tower, EnemyBase target)
    {
        var pool = tower.Container.Resolve(typeof(IProjectilePool)) as IProjectilePool;
        var proj = pool.Spawn(tower.transform.position, projectilePrefab);
        proj.Init(target, tower, tower.Data.damage, pool);
    }

    // OnEnemyHit is NOT called here. 
    // It is called by Tower.cs when the Projectile callback happens.
}