using System.Collections.Generic;
using UnityEngine;

public class PickClosest : ITargetingStrategy
{
    public Enemy SelectTarget(IEnumerable<Enemy> candidates, Tower tower)
    {
        Enemy closest = null;
        float distance = float.PositiveInfinity;

        foreach (Enemy e in candidates)
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
