using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantAttack : IAttackBehavior
{
    public void Execute(Tower tower, EnemyBase target)
    {
        //Instant attacks are done in the "Fire" event.
        if (tower.Data.ability != null)
            tower.Data.ability.OnFire(tower, target);
    }
}
