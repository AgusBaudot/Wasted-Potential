using UnityEngine;

public interface IEnemyFactory
{
    Enemy Create(Vector3 spawnPosition, IEnemyFactory originFactory = null);
    void Release(Enemy enemy);
}