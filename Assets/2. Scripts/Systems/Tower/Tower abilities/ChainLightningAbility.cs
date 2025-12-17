using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Chain Lightning")]
public class ChainLightningAbility : InstantAttack
{
    [Header("Chain Settings")]
    public int bounceCount = 3;
    public float bounceRange = 4;
    [Range(0, 1)] public float damageDecay = 0.2f;
    public LayerMask enemyLayer; // <--- VITAL for the buffer!

    [Header("Visuals")]
    public LineRenderer lightningPrefab;

    // Fixed buffer size is fine if we filter by LayerMask
    private static readonly Collider2D[] _resultsBuffer = new Collider2D[20];

    public override void Fire(Tower tower, EnemyBase target)
    {
        HashSet<int> visitedInstanceIDs = new HashSet<int>();
        
        EnemyBase currentTarget = target;
        EnemyBase previousTarget = null; // Track previous for visuals
        float currentDamage = tower.Data.damage;

        // Start from the tower for the first visual
        Vector3 previousPosition = tower.transform.position;

        for (int i = 0; i <= bounceCount; i++)
        {
            if (currentTarget == null || !currentTarget.IsAlive) break;

            // --- 1. VISUALS FIX ---
            // Draw from previous position to the current target
            SpawnVisuals(previousPosition, currentTarget.transform.position);
            
            // Update previous position for the NEXT loop
            previousPosition = currentTarget.transform.position; 

            // --- 2. DAMAGE FIX ---
            // Apply the SPECIFIC calculated damage, not the generic tower damage
            currentTarget.ApplyDamage(Mathf.CeilToInt(currentDamage), tower.gameObject);
            
            // Apply Status manually (since we aren't using OnEnemyHit for damage)
            if (tower.Data.onHitStatus != null)
                currentTarget.ApplyStatus(tower.Data.onHitStatus);

            // Mark visited
            visitedInstanceIDs.Add(currentTarget.gameObject.GetInstanceID());

            // Decay damage for NEXT hit
            currentDamage *= (1f - damageDecay);

            // Find next
            currentTarget = FindNextTarget(currentTarget.transform.position, visitedInstanceIDs);
        }
    }

    private EnemyBase FindNextTarget(Vector3 origin, HashSet<int> visited)
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(origin, bounceRange, _resultsBuffer);

        EnemyBase nearestEnemy = null;
        float shortestSqrDist = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D col = _resultsBuffer[i];
            
            if (!col.TryGetComponent(out EnemyBase enemy)) continue;

            if (!enemy.IsAlive || visited.Contains(enemy.gameObject.GetInstanceID())) continue;

            float sqrDist = (col.transform.position - origin).sqrMagnitude;

            if (sqrDist < shortestSqrDist)
            {
                shortestSqrDist = sqrDist;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void SpawnVisuals(Vector3 start, Vector3 end)
    {
        if (lightningPrefab == null) return;
        
        // Minor visual polish: Add a random offset to the middle of the line 
        // later to make it look jagged! For now, a straight line is fine.
        var line = Instantiate(lightningPrefab, start, Quaternion.identity);
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        Destroy(line.gameObject, 0.2f);
    }
}