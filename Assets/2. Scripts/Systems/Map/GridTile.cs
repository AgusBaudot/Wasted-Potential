using System.Collections.Generic;
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

    public Vector2Int Next
    {
        get
        {
            // if (nexts.Count == 0) return nexts[0];
            currentIndex++;
            if (currentIndex >= nexts.Count) currentIndex = 0;
            return nexts[currentIndex];
        }
    }

    private List<Vector2Int> nexts = new List<Vector2Int>();
    private int currentIndex = -1;

    public GridTile(Vector2Int pos, GridTileType type)
    {
        GridPosition = pos;
        SetType(type);
    }

    public void SetType(GridTileType type)
    {
        Type = type;
        Walkable = type == GridTileType.Path/* || type == GridTileType.Goal || type == GridTileType.Spawn*/;
        Buildable = type == GridTileType.Buildable;
    }

    public void SetNext(Vector2Int next) => nexts.Add(next);

    public void ClearNext() => nexts.Clear();
    
    public List<Vector2Int> GetAllNexts() => nexts;
}