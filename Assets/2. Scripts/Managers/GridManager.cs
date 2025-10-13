using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager  Instance { get; private set;  }
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase pathTile;
    [SerializeField] private TileBase buildableTile;
    [SerializeField] private TileBase blockedTile;
    [SerializeField] private TileBase spawnTile;
    [SerializeField] private TileBase goalTile;

    public GridTile SpawnTile { get; private set; }
    public GridTile GoalTile { get; private set; }

    private Dictionary<Vector2Int, GridTile> _tiles;

    private void Awake()
    {
        Instance = this;
        BuildGridFromTilemap();
        CalculatePath();
    }

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
                _ when tileBase == pathTile => GridTileType.Path,
                _ when tileBase == buildableTile => GridTileType.Buildable,
                _ when tileBase == blockedTile => GridTileType.Blocked,
                _ when tileBase == spawnTile => GridTileType.Spawn,
                _ when tileBase == goalTile => GridTileType.Goal,
                _ => GridTileType.Empty  // A default value in case none match
            };

            var tile = new GridTile(gridPos, type);
            _tiles[gridPos] = tile;

            if (type == GridTileType.Spawn) SpawnTile = tile;
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
        PathFinder finder = new PathFinder();
        var walkables = _tiles.Values.Where(a => a.Walkable).Select(a => a.GridPosition);
        var mappedTiles = finder.CalculatePath(GoalTile.GridPosition, walkables);
        foreach (var kvp in mappedTiles)
        {
            GridTile tile = GetTile(kvp.Key);
            tile.SetNext(kvp.Value.nextPos);

            if (kvp.Value.isLeaf)
            {
                SearchForSpawn(kvp.Key);
            }
        }        
    }

    private void SearchForSpawn(Vector2Int position)
    {
        //Check 4 adjacent tiles and set spawn.next to this incoming tile.
    }
}