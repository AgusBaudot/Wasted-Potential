using UnityEngine;

public abstract class TowerAbility : ScriptableObject
{
    public virtual void OnPlaced(Tower tower) { }

    public virtual void OnTick(Tower tower, float deltaTime) { }

    public virtual void OnFire(Tower tower, GameObject target) { }

    public virtual bool OnEnemyHit(Tower tower, EnemyBase enemy/*, Projectile projectile = null*/) { return false; }
}