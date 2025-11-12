using System.Collections.Generic;
using UnityEngine;

public class PickClosest : ITargetingStrategy
{
    public EnemyBase SelectTarget(IEnumerable<EnemyBase> candidates, Tower tower)
    {
        EnemyBase closest = null;
        float distance = float.PositiveInfinity;

        foreach (EnemyBase e in candidates)
        {
            if (e == null || !e.IsAlive) continue;

            var distSq = (e.transform.position - tower.transform.position).sqrMagnitude;

            if (distSq < distance)
            {
                distance = distSq;
                closest = e;
            }
        }
        return closest;
    }
}
