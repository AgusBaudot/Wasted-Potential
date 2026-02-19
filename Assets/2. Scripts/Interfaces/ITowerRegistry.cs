using System;
using System.Collections.Generic;

public interface ITowerRegistry
{
    public event Action<Tower> OnTowerAdded;
    public event Action<Tower> OnTowerRemoved;

    public IReadOnlyList<Tower> AllTowers => new List<Tower>();
    
    public void RegisterTower(Tower tower);

    public void UnregisterTower(Tower tower);
}