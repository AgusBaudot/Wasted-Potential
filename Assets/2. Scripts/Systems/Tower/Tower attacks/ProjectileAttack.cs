using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : IAttackBehavior
{
    public void Execute(Tower tower, EnemyBase target)
    {
        // Ability hook
        tower.Data.ability?.OnFire(tower, target);

        // Spawn projectile
        var proj = ServiceLocator.Get<ProjectilePool>().Spawn(tower.transform.position);
        proj.Init(target, tower, tower.Data.damage);
    }
}
