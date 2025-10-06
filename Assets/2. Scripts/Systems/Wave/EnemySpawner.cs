using System;
using UnityEngine;

[RequireComponent(typeof(FactoryProvider))]
public class EnemySpawner : MonoBehaviour
{
    private FactoryProvider _factoryProvider;
    private IEnemyFactory _enemyFactory;
    private GridManager _grid;

    private void Awake()
    {
        _factoryProvider = GetComponent<FactoryProvider>();
        _enemyFactory = _factoryProvider.GetFactory(EnemyType.Bear);
    }

    public Enemy Spawn(Vector2Int gridPos)
    {
        _grid??= GridManager.Instance;
        Vector3 world = _grid.GridToWorld(gridPos);
        return Spawn(world);
    }

    public Enemy Spawn(Vector3 worldPos)
    {
        _grid??= GridManager.Instance;
        var enemy = _enemyFactory.Create(worldPos, _enemyFactory);
        return enemy;
    }

    public void Release(Enemy enemy)
    {
        //Spawner-specific cleanup if needed in future (VFX, sound), then:
        enemy.ReturnToPool();
    }
}