using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private readonly HashSet<EnemyBase> _enemies = new();

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    public void RegisterEnemy(EnemyBase enemy)
    {
        _enemies.Add(enemy);
        enemy.OnRemoved += UnregisterEnemy;
    }

    private void UnregisterEnemy(EnemyBase enemy)
    {
        _enemies.Remove(enemy);
        enemy.OnRemoved -= UnregisterEnemy;
    }

    public List<EnemyBase> GetAllEnemies() => new List<EnemyBase>(_enemies);

    public List<EnemyBase> GetEnemiesInRange(Vector3 position, float range)
    {
        var list = new List<EnemyBase>();
        foreach (var enemy in _enemies)
        {
            if (!enemy.IsAlive) continue;
            if ((enemy.transform.position - position).sqrMagnitude <= range * range)
                list.Add(enemy);
        }
        return list;
    }
}