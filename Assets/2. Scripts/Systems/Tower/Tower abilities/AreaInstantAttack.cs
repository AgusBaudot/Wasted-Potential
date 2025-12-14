using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Area Instant Attack")]
public class AreaInstantAttack : InstantAttack
{
    public override void Fire(Tower tower, EnemyBase target)
    {
        Collider2D [] hits = Physics2D.OverlapBoxAll(tower.transform.position, Vector2.one * tower.Data.range, 0);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyBase enemy))
            {
                //1. Deal damage
                enemy.ApplyDamage(tower.Data.damage, tower.gameObject);
                
                //2. Apply status (Generic)
                //This can apply Stun, Poison, or whatever is in the CardData.
                if (tower.Data.onHitStatus != null)
                {
                    enemy.ApplyStatus(tower.Data.onHitStatus);
                }
            }
        }
    }
}