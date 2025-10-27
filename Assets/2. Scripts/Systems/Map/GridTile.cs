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

    public Vector2 Center { get; private set; }

    public Vector2Int Next
    {
        get
        {
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
        Center = pos + Vector2.one / 2;
    }

    public void SetType(GridTileType type)
    {
        Type = type;
        Walkable = type == GridTileType.Path || type == GridTileType.Goal || type == GridTileType.Spawn;
        Buildable = type == GridTileType.Buildable;
    }

    public void SetNext(Vector2Int next) => nexts.Add(next);

    public void ClearNext()
    {
        nexts.Clear();
        currentIndex = -1;
    }

    public List<Vector2Int> GetAllNexts() => nexts;

    // public bool HasNext(Vector2Int candidate) => nexts.Contains(candidate);
    
    public void SetBuildable(bool buildable) => Buildable = buildable;
}