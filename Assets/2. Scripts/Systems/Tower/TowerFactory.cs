using UnityEngine;

public class TowerFactory : MonoBehaviour, ITowerFactory
{
    [SerializeField] private Transform towersParent;

    public bool TryCreate(CardData card, Vector3 worldPos, Vector2Int gridPos, out Tower createdTower)
    {
        createdTower = null;
        if (card == null || card.towerPrefab == null) return false;
        
        var go = Instantiate(card.towerPrefab, worldPos, Quaternion.identity, towersParent);
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
