using UnityEngine;

public interface IPoolable
{
    void Initialize(Vector3 spawnPosition, IEnemyFactory originFactory = null);
    void Reset();
}