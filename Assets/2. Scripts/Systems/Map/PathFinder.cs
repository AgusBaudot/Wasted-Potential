using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pathfinder that produces a flow-field-like mapping from walkable tiles toward a single goal.
/// It runs a frontier-based BFS (goal -> spawn direction) that collects ALL parents for each child
/// discovered in the same BFS layer. This preserves splits (multiple valid next-tiles per tile)
/// without storing per-tile numeric distances.
/// </summary>

public class PathFinder
{
    /// <summary>
    /// Calculates the next-map and outer frontier (spawn-neighbor candidates).
    /// - goal: the tile coordinate of the goal (seed).
    /// - walkableTiles: collection of positions considered walkable / part of path graph.
    ///
    /// Returns:
    /// - mappedPos: dictionary childPos -> list of parentPos (parents are one step closer to goal)
    /// - spawnNeighbors: list of frontier tiles (from which no new children were found) — useful for locating spawns
    /// </summary>
    public (Dictionary<Vector2Int, List<Vector2Int>> mappedPos, List<Vector2Int> spawnNeighbors)
        CalculatePath(Vector2Int goal, IEnumerable<Vector2Int> walkableTiles)
    {
        // Build a fast lookup set of all walkable positions (O(1) membership).
        var lookup = new HashSet<Vector2Int>(walkableTiles);

        // Result mapping: for each tile (child) list the parents (neighbor tiles one step closer to goal).
        var resultNext = new Dictionary<Vector2Int, List<Vector2Int>>();

        // Visited/discovered set (we mark a tile visited only when we promote it from newlyFound -> frontier)
        var visited = new HashSet<Vector2Int>();

        // If the goal itself is not in the walkable set, still treat it as discovered seed.
        visited.Add(goal);

        // Current frontier: list of tiles at current BFS depth (start with goal).
        var currentFrontier = new List<Vector2Int> { goal };

        // Collection to hold frontier tiles that had no children (i.e., candidate spawn-adjacent tiles).
        var spawnNeighbors = new List<Vector2Int>();

        // Neighbor direction order (deterministic). You can change the order to affect tie-breaking.
        // Common choice for 4-way grid: Right, Up, Left, Down or any consistent ordering.
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down
        };

        // Main loop: process layer by layer.
        while (currentFrontier.Count > 0)
        {
            // Map child -> list of parents discovered *this level*
            var newlyFound = new Dictionary<Vector2Int, List<Vector2Int>>();

            // Track which parents actually discovered at least one child this level.
            var parentsWithChildren = new HashSet<Vector2Int>();

            // For each parent in the current frontier, try to discover children (neighbors that are walkable and not yet visited).
            foreach (var parent in currentFrontier)
            {
                // Track if this parent added any children. We'll use this to detect leaves.
                bool parentAddedAnyChild = false;

                // Check neighbors in deterministic order
                foreach (var dir in directions)
                {
                    var child = parent + dir;

                    // Must be walkable and not yet discovered (visited).
                    if (!lookup.Contains(child) || visited.Contains(child))
                        continue;

                    // Record that parent discovered this child
                    parentAddedAnyChild = true;

                    // Append parent to newlyFound[child] (create list if necessary)
                    if (!newlyFound.TryGetValue(child, out var parentList))
                    {
                        parentList = new List<Vector2Int>(2); // small initial capacity
                        newlyFound.Add(child, parentList);
                    }
                    parentList.Add(parent);
                }

                if (parentAddedAnyChild)
                    parentsWithChildren.Add(parent);
            }

            // If no children were discovered from the entire frontier, then every parent in this frontier is a leaf
            // (outer-most tile). Those are candidate spawn-neighbors (tiles adjacent to spawns).
            if (newlyFound.Count == 0)
            {
                // Add all current frontier tiles as spawn-neighbors (they had no undiscovered walkable neighbors).
                foreach (var parent in currentFrontier)
                    spawnNeighbors.Add(parent);

                // We're done (no further expansion possible).
                break;
            }

            // Promote newlyFound keys to discovered/visited and incorporate parent lists into the result map.
            var nextFrontier = new List<Vector2Int>(newlyFound.Count);
            foreach (var kvp in newlyFound)
            {
                Vector2Int childPos = kvp.Key;
                List<Vector2Int> parentsThisLevel = kvp.Value;

                // If result already contains some parents from previous runs (unlikely in a fresh run),
                // merge lists. Usually resultNext won't contain the key yet.
                if (resultNext.TryGetValue(childPos, out var existingParents))
                {
                    existingParents.AddRange(parentsThisLevel);
                }
                else
                {
                    resultNext.Add(childPos, new List<Vector2Int>(parentsThisLevel));
                }

                // Mark discovered & add to next frontier
                visited.Add(childPos);
                nextFrontier.Add(childPos);
            }

            // Advance to the next layer
            currentFrontier = nextFrontier;
        }

        return (mappedPos: resultNext, spawnNeighbors: spawnNeighbors);
    }
}