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
                OnEnemyHit(tower, enemy);
            }
        }
    }
}