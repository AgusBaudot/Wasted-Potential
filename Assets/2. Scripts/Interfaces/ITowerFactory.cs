using UnityEngine;

public interface ITowerFactory
{
    bool TryCreate(CardData card, Vector3 worldPos, Vector2Int gridPos, out Tower createdTower);
    void Release(Tower tower);
}
