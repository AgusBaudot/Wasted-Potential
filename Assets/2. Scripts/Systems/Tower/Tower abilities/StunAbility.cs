using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Stun")]
public class StunAbility : TowerAbility
{
    public float stunDuration = 1.5f;

    public override void OnFire(Tower tower, EnemyBase target)
    {
        target.ApplyDamage(tower.Data.damage, tower.gameObject);
        target.ApplyStun(stunDuration);
    }
}