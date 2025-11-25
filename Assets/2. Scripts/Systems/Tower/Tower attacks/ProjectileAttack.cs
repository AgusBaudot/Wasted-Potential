using UnityEngine;

[CreateAssetMenu(menuName = "TD/Attacks/Projectile attack")]
public class ProjectileAttack : TowerAbility
{
    public Projectile projectilePrefab;

    public override void Fire(Tower tower, EnemyBase target)
    {
        // 1. Spawn
        var proj = ServiceLocator.Get<ProjectilePool>().Spawn(tower.transform.position, projectilePrefab);

        // 2. Initialize
        // Note: We pass 'tower.Data.damage' to the projectile so it knows how hard to hit
        proj.Init(target, tower, tower.Data.damage);
    }

    // OnEnemyHit is NOT called here. 
    // It is called by Tower.cs when the Projectile callback happens.
}