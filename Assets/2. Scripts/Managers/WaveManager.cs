using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> waves = new();
    [SerializeField] private EnemySpawner spawner;

    public event Action<int> OnWaveStarted; //Wave index
    public event Action<int> OnWaveCompleted;
    public event Action AllWavesCompleted;

    private int _currentWaveIndex = -1;
    private int _aliveCount = 0;
    private bool _isRunning = false;

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
            yield return new WaitForSeconds(wave.startDelay);

            _aliveCount = wave.enemyCount;
            
            //Spawn loop
            for (int s = 0; s < wave.enemyCount; s++)
            {
                SpawnAndTrack();
                yield return new WaitForSeconds(wave.spawnInterval);
            }
            
            //Wait until all spawned enemies are gone (killed or reached goal)
            while (_aliveCount > 0)
                yield return null;
            
            OnWaveCompleted?.Invoke(i);
        }
        
        AllWavesCompleted?.Invoke();
        _isRunning = false;
    }

    private void SpawnAndTrack()
    {
        //Spawn at grid spawn tile by default (customizable if needed)
        var spawnGridPos = GridManager.Instance.SpawnTile[0].GridPosition;
        var enemy = spawner.Spawn(spawnGridPos);
        
        //Subscribe to removal events
        enemy.OnRemoved += HandleEnemyRemoved;
    }

    private void HandleEnemyRemoved(Enemy enemy)
    {
        //Unsubscribe, decrement alive count, and return to pool via spawner.
        enemy.OnRemoved -= HandleEnemyRemoved;

        _aliveCount = Mathf.Max(0, _aliveCount - 1);

        spawner.Release(enemy);
    }

    public void StopWaves()
    {
        StopAllCoroutines();
        _isRunning = false;
    }
}