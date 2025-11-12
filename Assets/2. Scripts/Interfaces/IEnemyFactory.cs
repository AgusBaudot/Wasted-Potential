using UnityEngine;

public interface IEnemyFactory
{
    EnemyBase Create(Vector3 spawnPosition, IEnemyFactory originFactory = null);
    void Release(EnemyBase enemy);
}