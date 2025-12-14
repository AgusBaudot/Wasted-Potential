using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Chain Lightning")]
public class ChainLightningAbility : InstantAttack
{
    [Header("Chain Settings")]
    public int bounceCount = 3;
    public float bounceRange = 4; //Radius to search for next target
    [Range(0, 1)] public float damageDecay = 0.2f; //20% less damage per bounce.
    
    [Header("Visuals")]
    public LineRenderer lightningPrefab;
    
    //Optimization: static buffer to avoid allocating memory every frame.
    //20 is the max enemies checked around the bounce target.
    private static readonly Collider2D[] _resultsBuffer = new Collider2D[20];

    public override void Fire(Tower tower, EnemyBase target)
    {
        //1. Setup tracking
        HashSet<int> visitedInstanceIDs = new HashSet<int>();

        EnemyBase currentTarget = target;
        int currentDamage = tower.Data.damage;
        
        //2. Start the chain loop.
        for (int i = 0; i <= bounceCount; i++)
        {
            if (currentTarget == null || !currentTarget.IsAlive) break;
            
            //Apply Hit (Damage + Status)
            OnEnemyHit(tower, currentTarget);
            
            //Mark as visited so we don't bounce back to it
            visitedInstanceIDs.Add(currentTarget.gameObject.GetInstanceID());
            
            //Visuals: Spawn a lighning bolt from previos pos (or tower) to current
            Vector3 startPos = i == 0 ? tower.transform.position : currentTarget.transform.position;
            SpawnVisuals(startPos, currentTarget.transform.position);
            
            //Prepare for next bounce
            //Reduce damage for next hit
            currentDamage = Mathf.FloorToInt(currentDamage * (1f - damageDecay));
            
            //Find next target
            currentTarget = FindNextTarget(currentTarget.transform.position, visitedInstanceIDs);
        }
    }

    private EnemyBase FindNextTarget(Vector3 origin, HashSet<int> visited)
    {
        //Optimization 1: NonAlloc writes into pre-allocated _resultsBuffer
        int hitCount = Physics2D.OverlapCircleNonAlloc(origin, bounceRange, _resultsBuffer);

        EnemyBase nearestEnemy = null;
        float shortestSqrDist = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D col = _resultsBuffer[i];

            //Skip if not an enemy
            if (!col.TryGetComponent(out EnemyBase enemy)) continue;
            
            //Sip if dead or already hit in this chain
            if (!enemy.IsAlive || visited.Contains(enemy.gameObject.GetInstanceID())) continue;
            
            //Optimization 2: use sqrMagnitude (avoids expensive sqrt)
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

        var line = Instantiate(lightningPrefab, start, quaternion.identity);
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        Destroy(line.gameObject, 0.2f); //Quick flash
    }
}