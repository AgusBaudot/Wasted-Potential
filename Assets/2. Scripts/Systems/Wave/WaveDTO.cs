using System;

[Serializable]
public class WavesJsonWrapper
{
    public WaveDTO[] waves;
}

[Serializable]
public class WaveDTO
{
    public float startDelay;
    public int[] defaultSpawnDistribution;
    public SpawnEntryDTO[] entries;
}

[Serializable]
public class SpawnEntryDTO
{
    public string enemyType;
    public int count = 1;
    public float interval = 0.5f;
    public float startDelay = 0;
    public int[] spawnDistribution;
    public int weight = 100;
}