using System;
using System.Collections.Generic;
using UnityEngine;

public static class WaveJsonLoader
{
    //Load from Resources/WaveConfigs/waves.json
    public static List<Wave> LoadWavesFromResources(string resourcePath = "WaveConfigs/waves")
    {
        var textAsset = Resources.Load<TextAsset>(resourcePath);
        if (textAsset == null)
        {
            Debug.LogError($"WaveJsonLoader: could not find TextAsset at Resources/{resourcePath}.json");
            return new List<Wave>();
        }

        return ConvertFromJson(textAsset.text);
    }

    private static List<Wave> ConvertFromJson(string json)
    {
        var wrapper = JsonUtility.FromJson<WavesJsonWrapper>(json);
        if (wrapper == null || wrapper.waves == null)
            return new List<Wave>();

        var result = new List<Wave>(wrapper.waves.Length);

        foreach (var wDTO in wrapper.waves)
        {
            var wave = new Wave
            {
                startDelay = wDTO.startDelay,
                defaultSpawnDistribution = wDTO.defaultSpawnDistribution,
                entries = new List<SpawnEntry>()
            };

            if (wDTO.entries != null)
            {
                foreach (var eDTO in wDTO.entries)
                {
                    //Parse EnemyType enum
                    if (!Enum.TryParse<EnemyType>(eDTO.enemyType, out var enemyType))
                    {
                        Debug.LogWarning($"WaveJsonLoader: unknown EnemyType '{eDTO.enemyType}', defaulting to Beaver");
                        enemyType = EnemyType.Beaver;
                    }

                    var entry = new SpawnEntry
                    {
                        enemyType = enemyType,
                        count = eDTO.count,
                        interval = eDTO.interval,
                        spawnDistribution = eDTO.spawnDistribution,
                    };

                    wave.entries.Add(entry);
                }
            }

            result.Add(wave);
        }

        return result;
    }
}