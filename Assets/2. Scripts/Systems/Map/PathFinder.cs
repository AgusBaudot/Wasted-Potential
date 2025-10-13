using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private HashSet<Vector2Int> visited;
    private HashSet<Vector2Int> lookup;
    private Dictionary<Vector2Int, Vector2Int> next;
    
    /// <summary>
    /// CalculatePath returns a dictionary mapping tilePos, nextPos choosing neighbors based in this order: right, up, down, left.
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="walkableTiles"></param>
    /// <returns></returns>
    public Dictionary<Vector2Int, Vector2Int> CalculatePath(Vector2Int goal, IEnumerable<Vector2Int> walkableTiles)
    {
        lookup = new HashSet<Vector2Int>(walkableTiles);
        visited = new();
        next = new();
        var queue = new Queue<Vector2Int>();

        visited.Add(goal);
        queue.Enqueue(goal);
        
        while (queue.Count > 0) 
        {
            var current = queue.Dequeue();
            var neighbor = CheckForNeighbors(current);
            foreach (Vector2Int n in neighbor)
            {
                next[n] = current;
                visited.Add(n);

                queue.Enqueue(n);
            }
        }
        return next; //Return dictionary or dict.values?
    }

    private List<Vector2Int> CheckForNeighbors(Vector2Int tile)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>(3);
        var candidate = tile + Vector2Int.right;
        if (lookup.Contains(candidate) && !visited.Contains(candidate))
        {
            neighbors.Add(candidate);
        }

        candidate = tile + Vector2Int.up;
        if (lookup.Contains(candidate) && !visited.Contains(candidate))
        {
            neighbors.Add(candidate);
        }
        
        candidate = tile + Vector2Int.down;
        if (lookup.Contains(candidate) && !visited.Contains(candidate))
        {
            neighbors.Add(candidate);
        }
        
        candidate = tile + Vector2Int.left;
        if (lookup.Contains(candidate) && !visited.Contains(candidate))
        {
            neighbors.Add(candidate);
        }
        
        return neighbors;
    }
}
