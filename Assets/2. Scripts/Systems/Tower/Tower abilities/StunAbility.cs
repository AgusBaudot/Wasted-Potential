using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Stun")]
public class StunAbility : InstantAttack
{
    public float stunDuration = 1.5f;

    public override void Fire(Tower tower, EnemyBase target)
    {
        Collider[] hits = Physics.OverlapBox(tower.transform.position, Vector3.one * tower.Data.range, Quaternion.identity, 10);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyBase enemy))
            {
                // Everyone gets damaged
                enemy.ApplyDamage(tower.Data.damage, tower.gameObject);
                enemy.ApplyStun(stunDuration);
            }
        }
    }
}