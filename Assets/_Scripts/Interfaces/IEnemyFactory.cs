using UnityEngine;

public interface IEnemyFactory
{
    Enemy Create(Vector3 spawnPosition);
    void Release(Enemy enemy);
}