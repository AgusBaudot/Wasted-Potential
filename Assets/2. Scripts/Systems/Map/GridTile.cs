using UnityEngine;

public enum GridTileType
{
    Empty,
    Path,
    Buildable,
    Blocked,
    Spawn,
    Goal
}

public class GridTile
{
    public Vector2Int GridPosition { get; private set; }
    public bool Walkable { get; private set; }
    public bool Buildable { get; private set; }
    public GridTileType Type { get; private set; }
    public Vector2Int Next { get; private set; }

    public GridTile(Vector2Int pos, GridTileType type)
    {
        GridPosition = pos;
        SetType(type);
    }

    public void SetType(GridTileType type)
    {
        Type = type;
        Walkable = type == GridTileType.Path || type == GridTileType.Goal || type == GridTileType.Spawn;
        Buildable = type == GridTileType.Buildable;
    }

    public void SetNext(Vector2Int next) => Next = next;
}