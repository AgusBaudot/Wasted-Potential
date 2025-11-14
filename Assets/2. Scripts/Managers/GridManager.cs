using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager  Instance { get; private set;  }
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] pathTile;
    [SerializeField] private TileBase[] buildableTile;
    [SerializeField] private TileBase[] blockedTile;
    [SerializeField] private TileBase[] spawnTile;
    [SerializeField] private TileBase[] goalTile;

    public List<GridTile> SpawnTile = new List<GridTile>();
    public GridTile GoalTile { get; private set; }

    private Dictionary<Vector2Int, GridTile> _tiles;

    private void Awake()
    {
        ServiceLocator.Register(this);
        Instance = this;
        BuildGridFromTilemap();
        CalculatePath();
    }

    private void OnDestroy() => ServiceLocator.Unregister(this);

    private void BuildGridFromTilemap()
    {
        _tiles = new Dictionary<Vector2Int, GridTile>();

        BoundsInt bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int cellPos = new Vector3Int(pos.x, pos.y, 0);
            TileBase tileBase = tilemap.GetTile(cellPos);
            if (tileBase == null) continue;

            Vector2Int gridPos = new Vector2Int(pos.x, pos.y);
            
            
            GridTileType type = tileBase switch
            {
                _ when pathTile.Contains(tileBase) => GridTileType.Path,
                _ when buildableTile.Contains(tileBase) => GridTileType.Buildable,
                _ when blockedTile.Contains(tileBase) => GridTileType.Blocked,
                _ when spawnTile.Contains(tileBase) => GridTileType.Spawn,
                _ when goalTile.Contains(tileBase) => GridTileType.Goal,
                _ => GridTileType.Empty  // A default value in case none match
            };

            var tile = new GridTile(gridPos, type);
            _tiles[gridPos] = tile;

            if (type == GridTileType.Spawn) SpawnTile.Add(tile);
            if (type == GridTileType.Goal) GoalTile = tile;
        }
    }

    public GridTile GetTile(Vector2Int gridPos)
    {
        _tiles.TryGetValue(gridPos, out GridTile tile);
        return tile;
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return tilemap.CellToWorld(new Vector3Int(gridPos.x, gridPos.y, 0))
               + tilemap.cellSize / 2;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return (Vector2Int)tilemap.WorldToCell(worldPos);
    }

    private void CalculatePath()
    {
        // Call the pathfinder and get the dictionary it returns:
        PathFinder finder = new PathFinder();
        var walkables = _tiles.Values.Where(a => a.Walkable).Select(a => a.GridPosition);
        var spawns = SpawnTile.Select(a => a.GridPosition);

        var mappedPaths = finder.CalculatePath(GoalTile.GridPosition, walkables, spawns);

        // Clear previous nexts
        foreach (var tile in _tiles.Values)
            tile.ClearNext();

        // Map directed edges current -> list(next) into GridTile.SetNext
        foreach (var kvp in mappedPaths)
        {
            var currentPos = kvp.Key;
            var nextPositions = kvp.Value;

            var currentTile = GetTile(currentPos);
            if (currentTile == null)
                continue; // skip missing tiles

            foreach (var nextPos in nextPositions)
            {
                // If SetNext accepts Vector2Int positions (your original code used this),
                // pass the position directly. If it expects a GridTile, call GetTile(nextPos).
                currentTile.SetNext(nextPos);
            }
        }
    }

    private void SearchForSpawn(Vector2Int position)
    {
        //Candidate is one of 4 adjacent tiles that is spawn tile.
        var candidate = GetTile(position + Vector2Int.up);
        if (candidate?.Type == GridTileType.Spawn)
            candidate.SetNext(position);
        
        candidate = GetTile(position + Vector2Int.right);
        if (candidate?.Type == GridTileType.Spawn)
            candidate.SetNext(position);
        
        candidate = GetTile(position + Vector2Int.down);
        if (candidate?.Type == GridTileType.Spawn)
            candidate.SetNext(position);
        
        candidate = GetTile(position + Vector2Int.left);
        if (candidate?.Type == GridTileType.Spawn)
            candidate.SetNext(position);

    }

    private void OnDrawGizmosSelected()
    {
        if (_tiles == null || _tiles.Count == 0) return;
        
        var walkables = _tiles.Values.Where(a => a.Walkable).ToArray();
        foreach (GridTile tile in walkables)
        {
            Gizmos.color = Color.white;
            foreach (Vector2Int nexts in tile.GetAllNexts())
            {
                Gizmos.DrawLine(GridToWorld(tile.GridPosition), GridToWorld(nexts));
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(GridToWorld(nexts), 0.1f);
            }
        }
    }

    // Put this in your GridManager / WaveManager class

    private void DebugDumpMappedPaths(Dictionary<Vector2Int, List<Vector2Int>> mappedPaths, Vector2Int goal, IEnumerable<Vector2Int> spawns)
    {
        if (mappedPaths == null)
        {
            Debug.Log("DebugDumpMappedPaths: mappedPaths is null");
            return;
        }

        Debug.Log($"DebugDumpMappedPaths: nodes={mappedPaths.Count}, goal={goal}, spawns={string.Join(", ", spawns.Select(s => s.ToString()))}");

        // Print every mapping (current -> [next,next,...])
        foreach (var kvp in mappedPaths.OrderBy(k => k.Key.x).ThenBy(k => k.Key.y))
        {
            var current = kvp.Key;
            var nexts = kvp.Value;
            Debug.Log($"  {current} -> [{string.Join(", ", nexts)}]");
        }

        // Validate references: every next should correspond to an existing tile (or be goal)
        int missingTiles = 0;
        foreach (var kvp in mappedPaths)
        {
            foreach (var next in kvp.Value)
            {
                if (next != goal && GetTile(next) == null)
                {
                    Debug.LogWarning($"MappedPaths references missing tile at {next} (from {kvp.Key})");
                    missingTiles++;
                }
            }
        }
        Debug.Log($"DebugDumpMappedPaths: missingTiles={missingTiles}");
    }

}