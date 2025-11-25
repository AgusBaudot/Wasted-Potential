using UnityEngine;

[CreateAssetMenu(menuName = "TD/Attacks/Instant attack")]
public class InstantAttack : TowerAbility
{
    public GameObject visualVFX; // Optional visual

    public override void Fire(Tower tower, EnemyBase target)
    {
        // 1. Visuals
        if (visualVFX) Instantiate(visualVFX, tower.transform.position, Quaternion.identity);

        // 2. Logic: Since it's instant, we manually trigger the Hit phase immediately.
        OnEnemyHit(tower, target);
    }

    // We use the base OnEnemyHit (deals damage), 
    // or we can override it here to add "Chain Lightning" etc.
}