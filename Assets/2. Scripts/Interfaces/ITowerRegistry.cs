using System;
using System.Collections.Generic;

public interface ITowerRegistry
{
    event Action<Tower> OnTowerAdded;
    event Action<Tower> OnTowerRemoved;

    IReadOnlyList<Tower> AllTowers => new List<Tower>();
    
    void RegisterTower(Tower tower);
    void UnregisterTower(Tower tower);
}