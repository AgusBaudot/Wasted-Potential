using UnityEngine;

public interface IPoolable
{
    void Initialize(EnemyData data, Vector3 spawnPosition, IEnemyFactory originFactory = null);
    void Reset();
}