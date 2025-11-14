using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deterministic multi-run DFS pathfinder.
/// - Runs one DFS per permutation of the 4 cardinal directions (4! = 24 runs).
/// - For each permutation it runs a DFS starting from every spawn position.
/// - DFS always explores neighbors in the permutation's priority order (no distance checks).
/// - Records every unique directed edge discovered as mappedPaths[current] -> next (List of next-tiles).
/// - Goal is treated as a terminal and is allowed even if not listed in walkableTiles.
/// 
/// This class returns only the merged mappedPaths (current -> list of next tiles).
/// </summary>
public class PathFinder
{
    /// <summary>
    /// Runs the DFS-based path collection.
    /// - goal: tile coordinate of the goal (terminal).
    /// - walkableTiles: positions considered traversable.
    /// - spawnPositions: starting tiles to run DFS from.
    ///
    /// Returns:
    /// - mappedPaths: Dictionary currentPos -> List of nextPos (unique next-tiles discovered).
    /// </summary>
    public Dictionary<Vector2Int, List<Vector2Int>> CalculatePath(
        Vector2Int goal,
        IEnumerable<Vector2Int> walkableTiles,
        IEnumerable<Vector2Int> spawnPositions)
    {
        var walkable = new HashSet<Vector2Int>(walkableTiles ?? Array.Empty<Vector2Int>());
        var spawns = new HashSet<Vector2Int>(spawnPositions ?? Array.Empty<Vector2Int>());

        var mappedPaths = new Dictionary<Vector2Int, List<Vector2Int>>();

        // Canonical base directions to permute deterministically.
        Vector2Int[] baseDirs = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        var permutations = GeneratePermutations(baseDirs); // 24 deterministic permutations

        // For each permutation, run DFS from every spawn and record edges current -> next.
        foreach (var perm in permutations)
        {
            foreach (var spawn in spawns)
            {
                var visitedRun = new HashSet<Vector2Int>();
                var stack = new Stack<Vector2Int>();

                // Start from spawn even if not walkable (but traversal only moves into walkable tiles or goal).
                visitedRun.Add(spawn);
                stack.Push(spawn);

                while (stack.Count > 0)
                {
                    var current = stack.Pop();

                    // Do not expand the goal's neighbors (goal is terminal).
                    if (current == goal)
                        continue;

                    // Explore neighbors in permutation order.
                    foreach (var dir in perm)
                    {
                        var next = current + dir;

                        // Only step into next if it's walkable or it's the goal.
                        if (!walkable.Contains(next) && next != goal)
                            continue;

                        // Record directed edge current -> next.
                        if (!mappedPaths.TryGetValue(current, out var nextList))
                        {
                            nextList = new List<Vector2Int>(2);
                            mappedPaths.Add(current, nextList);
                        }
                        if (!nextList.Contains(next))
                            nextList.Add(next);

                        // If next is goal, mark edge but do not push/expand goal.
                        if (next == goal)
                            continue;

                        // Otherwise continue DFS (LIFO), avoiding cycles within this run.
                        if (visitedRun.Add(next))
                            stack.Push(next);
                    }
                } // end single spawn DFS for this permutation
            } // end foreach spawn
        } // end foreach permutation

        return mappedPaths;
    }

    /// <summary>
    /// Generate deterministic permutations of exactly 4 directions.
    /// </summary>
    private static List<Vector2Int[]> GeneratePermutations(Vector2Int[] dirs)
    {
        if (dirs == null || dirs.Length != 4)
            throw new ArgumentException("GeneratePermutations expects exactly 4 directions.");

        var result = new List<Vector2Int[]>();
        Permute(dirs, 0, result);
        return result;
    }

    private static void Permute(Vector2Int[] arr, int start, List<Vector2Int[]> outList)
    {
        if (start >= arr.Length)
        {
            var copy = new Vector2Int[arr.Length];
            Array.Copy(arr, copy, arr.Length);
            outList.Add(copy);
            return;
        }

        for (int i = start; i < arr.Length; i++)
        {
            Swap(arr, start, i);
            Permute(arr, start + 1, outList);
            Swap(arr, start, i);
        }
    }

    private static void Swap(Vector2Int[] arr, int i, int j)
    {
        var tmp = arr[i];
        arr[i] = arr[j];
        arr[j] = tmp;
    }
}
