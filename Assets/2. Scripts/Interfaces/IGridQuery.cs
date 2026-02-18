using UnityEngine;
using System.Collections.Generic;

public interface IGridQuery
{
    GridTile GetTile(Vector2Int position);
    Vector3 GridToWorld(Vector2Int position);
    Vector2Int WorldToGrid(Vector3 worldPosition);
    IReadOnlyList<GridTile> SpawnTiles { get; }
    GridTile GoalTile { get; }
}