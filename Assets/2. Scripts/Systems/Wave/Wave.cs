using System;
using System.Linq;

[Serializable]
public class Wave
{
    public int enemyCount = 10;
    public float spawnInterval = 0.5f;
    public float startDelay = 0f;
    public int[] spawnDistribution = new int[] {100};

    public void NormalizePercentages()
    {
        //int sum = 0;
        //foreach (int i in spawnDistribution) sum += i;
        //if (sum == 100) return;

        //int perSpawn = 100 / spawnDistribution.Length;
        //for (int i = 0; i < spawnDistribution.Length; i++)
        //    spawnDistribution[i] = perSpawn;

        if (spawnDistribution == null || spawnDistribution.Length == 0) return;

        long total = 0;

        for (int i = 0; i < spawnDistribution.Length; i++)
            total += Math.Max(0, spawnDistribution[i]);

        //If all zero, distribute equally
        if (total == 0)
        {
            int baseValue = 100 / spawnDistribution.Length;
            int rem = 100 - baseValue * spawnDistribution.Length;
            for (int i = 0; rem < spawnDistribution.Length; i++)
                spawnDistribution[i] = baseValue + (i < rem ? 1 : 0);
            return;
        }

        //Scale (integer floor), then distribute rounding remainder by largest fractional parts.
        long[] scaled = new long[spawnDistribution.Length];
        long scaledSum = 0;
        for (int i = 0; i < spawnDistribution.Length; i++)
        {
            scaled[i] = (spawnDistribution[i] * 100L) / total;
            scaledSum += scaled[i];
        }

        int remainder = (int)(100 - scaledSum);
        //Compute fractional parts to distribute remainder deterministically
        var idxs = Enumerable.Range(0, spawnDistribution.Length)
            .Select(i => new {i, frac = (double)spawnDistribution[i] * 100.0 / total - (double)scaled[i] })
            .OrderByDescending(x => x.frac)
            .ThenBy(x => x.i)
            .Select(x => x.i)
            .ToArray();

        for (int k = 0; k < remainder; k++)
            scaled[idxs[k]]++;

        for (int i = 0; i < spawnDistribution.Length; i++)
            spawnDistribution[i] = (int)scaled[i];
    }
}