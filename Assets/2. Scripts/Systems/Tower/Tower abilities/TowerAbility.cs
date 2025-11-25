using UnityEngine;

public abstract class TowerAbility : ScriptableObject
{
    public virtual void OnPlaced(Tower tower) { }

    public virtual void OnTick(Tower tower, float deltaTime) { }

    public abstract void Fire(Tower tower, EnemyBase target);

    public virtual void OnEnemyHit(Tower tower, EnemyBase target)
    {
        //Default behvaior: just deal damage.
        if (target is ITargetable t)
            t.ApplyDamage(tower.Data.damage, tower.gameObject);
    }
}