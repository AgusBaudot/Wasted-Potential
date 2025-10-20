using System.Collections.Generic;

public interface ITargetingStrategy
{
    Enemy SelectTarget(IEnumerable<Enemy> candidates, Tower tower);
}