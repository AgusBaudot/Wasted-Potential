using System.Collections.Generic;
using UnityEngine;

public interface IEnemyQuery
{
    public void RegisterEnemy(EnemyBase enemy);

    public List<EnemyBase> GetAllEnemies() => new List<EnemyBase>();

    public List<EnemyBase> GetEnemiesInRange(Vector3 position, float range);
}