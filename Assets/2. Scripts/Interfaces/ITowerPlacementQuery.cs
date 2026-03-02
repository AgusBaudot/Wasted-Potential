using UnityEngine;

public interface ITowerPlacementQuery
{
    bool TryPlaceTower(CardData card, Vector2Int gridPosition);
}