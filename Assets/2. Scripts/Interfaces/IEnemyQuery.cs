using System.Collections.Generic;
using UnityEngine;

public interface IEnemyQuery
{
    void RegisterEnemy(EnemyBase enemy);
    List<EnemyBase> GetAllEnemies();
    List<EnemyBase> GetEnemiesInRange(Vector3 position, float range);
}