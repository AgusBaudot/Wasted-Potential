using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class TowerManager : MonoBehaviour
{
    private List<Tower> _allTowers = new List<Tower>();

    public event Action<Tower> OnTowerAdded;
    public event Action<Tower> OnTowerRemoved;

    public IReadOnlyList<Tower> AllTowers => _allTowers;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    public void RegisterTower(Tower tower)
    {
        if (!_allTowers.Contains(tower))
        {
            _allTowers.Add(tower);
            OnTowerAdded?.Invoke(tower);
        }
    }

    public void UnregisterTower(Tower tower)
    {
        if (_allTowers.Remove(tower))
            OnTowerRemoved?.Invoke(tower);
    }
}
