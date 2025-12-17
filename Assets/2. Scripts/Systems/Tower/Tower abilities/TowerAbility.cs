using UnityEngine;

public abstract class TowerAbility : ScriptableObject
{
    
    public virtual void OnPlaced(Tower tower)
    {
        tower.GetComponent<AudioSource>().Play();
    }

    public virtual void OnTick(Tower tower, float deltaTime) { }

    public abstract void Fire(Tower tower, EnemyBase target);

    public virtual void OnEnemyHit(Tower tower, EnemyBase target)
    {
        //1. Standard damage
        if (target is ITargetable t)
        {
            t.ApplyDamage(tower.Data.damage, tower.gameObject);
        }
        
        //2. Standard status application.
        //The ability automatically checks if the CardData has a status to apply.
        if (tower.Data.onHitStatus != null)
        {
            target.ApplyStatus(tower.Data.onHitStatus);
        }
    }
}