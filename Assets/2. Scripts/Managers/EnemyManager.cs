using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private readonly HashSet<Enemy> _enemies = new();

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
        enemy.OnRemoved += UnregisterEnemy;
    }

    private void UnregisterEnemy (Enemy enemy)
    {
        _enemies.Remove(enemy);
        enemy.OnRemoved -= UnregisterEnemy;
    }

    public List<Enemy> GetAllEnemies() => new List<Enemy>(_enemies);

    public List<Enemy> GetEnemiesInRange(Vector3 position, float range)
    {
        var list = new List<Enemy>();
        foreach (var enemy in _enemies)
        {
            if (!enemy.IsAlive) continue;
            if ((enemy.transform.position - position).sqrMagnitude <= range * range)
                list.Add(enemy);
        }
        return list;
    }
}