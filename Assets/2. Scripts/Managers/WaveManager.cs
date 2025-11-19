using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> waves = new();
    [SerializeField] private EnemySpawner spawner;
    
    public List<Wave> AllWaves => waves;
        
    public event Action<int> OnWaveStarted; //Wave index
    public event Action<int> OnWaveCompleted;
    public event Action AllWavesCompleted;

    private int _currentWaveIndex = -1;
    private int _aliveCount = 0;
    private bool _isRunning = false;

    private void Awake()
    {
        ServiceLocator.Register(this);
        waves = WaveJsonLoader.LoadWavesFromResources("WaveConfigs/waves");
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    public void StartWaves()
    {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            var wave = waves[i];
            _currentWaveIndex = i;
            OnWaveStarted?.Invoke(i);
            Debug.Log($"Wave {i + 1} starting.");
            
            //Optional start delay.
            yield return Helpers.GetWait(wave.startDelay);

            _aliveCount = ComputeEnemyCount(wave);

            foreach (var entry in wave.entries)
                StartCoroutine(RunSpawnEntry(entry));

            while (_aliveCount > 0)
                yield return null;
            
            OnWaveCompleted?.Invoke(i);
        }
        
        AllWavesCompleted?.Invoke();
        _isRunning = false;
    }

    private IEnumerator RunSpawnEntry(SpawnEntry entry)
    {
        yield return Helpers.GetWait(entry.startDelay);

        for (int k = 0; k < entry.count; k++)
        {
            int[] dist = entry.spawnDistribution;
            if(dist == null || dist.Length == 0)
                dist = waves[_currentWaveIndex].defaultSpawnDistribution;
            
            int spawnIndex = GetRandomSpawnIndex(dist);
            var spawnGridPos = ServiceLocator.Get<GridManager>().SpawnTile[spawnIndex].GridPosition;

            var enemy = spawner.Spawn(entry.enemyType, spawnGridPos);
            enemy.OnRemoved += HandleEnemyRemoved;

            yield return Helpers.GetWait(entry.interval);
        }
    }

    private int ComputeEnemyCount(Wave wave)
    {
        int total = 0;
        foreach (var entry in wave.entries)
            total += entry.count;
        return total;
    }

    // private void SpawnAndTrack()
    // {
    //     int randomIndex = GetRandomSpawnIndex(waves[_currentWaveIndex].spawnDistribution);
    //     var spawnGridPos = ServiceLocator.Get<GridManager>().SpawnTile[randomIndex].GridPosition;
    //     var enemy = spawner.Spawn(spawnGridPos);
    //     
    //     //Subscribe to removal events
    //     enemy.OnRemoved += HandleEnemyRemoved;
    // }

    private void HandleEnemyRemoved(EnemyBase enemy)
    {
        //Unsubscribe, decrement alive count, and return to pool via spawner.
        enemy.OnRemoved -= HandleEnemyRemoved;

        _aliveCount = Mathf.Max(0, _aliveCount - 1);

        spawner.Release(enemy);
    }

    private int GetRandomSpawnIndex(int[] distribution)
    {
        int roll = UnityEngine.Random.Range(0, 100); //Actually 0-99
        int cumulative = 0;

        for (int i = 0; i < distribution.Length; i++)
        {
            cumulative += distribution[i];
            if (roll < cumulative)
                return i;
        }

        return distribution.Length - 1; //Return last just in case.
    }

    public void StopWaves()
    {
        StopAllCoroutines();
        _isRunning = false;
        _aliveCount = 0;
    }
}