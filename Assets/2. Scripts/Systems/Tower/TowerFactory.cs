using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour, ITowerFactory
{
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private Transform towersParent;


    public bool TryCreate(CardData card, Vector3 worldPos, Vector2Int gridPos, out Tower createdTower)
    {
        createdTower = null;
        if (towerPrefab == null) return false;
        
        var go = Instantiate(towerPrefab.gameObject, worldPos, Quaternion.identity, towersParent);
        createdTower = go.GetComponent<Tower>();
        if (createdTower == null)
        {
            Destroy(go);
            return false;
        }
        
        createdTower.Initialize(card, gridPos);
        return true;
    }

    public void Release(Tower tower)
    {
        if (tower == null) return;
        Destroy(tower.gameObject);
    }
}
