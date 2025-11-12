using System.Collections.Generic;

public interface ITargetingStrategy
{
    EnemyBase SelectTarget(IEnumerable<EnemyBase> candidates, Tower tower);
}