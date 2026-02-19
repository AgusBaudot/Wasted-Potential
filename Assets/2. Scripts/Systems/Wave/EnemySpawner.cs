using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(FactoryProvider))]
public class EnemySpawner : MonoBehaviour
{
    private FactoryProvider _factoryProvider;
    private IGridQuery _grid;
    private IEnemyQuery _enemyManager;
    private IEnemyFactory _enemyFactory;

    [Inject]
    public void Construct(IEnemyQuery enemyManager, IGridQuery gridManager)
    {
        _grid = gridManager ?? throw new ArgumentNullException(nameof(gridManager));
        _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
    }

    private void Awake()
    {
        _factoryProvider = GetComponent<FactoryProvider>();
        _enemyFactory = _factoryProvider.GetFactory(EnemyType.Bear);
    }

    public EnemyBase Spawn(EnemyType type, Vector2Int gridPos)
    {
        Vector3 world = _grid.GridToWorld(gridPos);
        return Spawn(type, world);
    }

    public EnemyBase Spawn(EnemyType type, Vector3 worldPos)
    {
        var factory = _factoryProvider.GetFactory(type);
        var enemy = factory.Create(worldPos, factory);

        _enemyManager.RegisterEnemy(enemy);

        return enemy;
    }

    public void Release(EnemyBase enemy)
    {
        //Spawner-specific cleanup if needed in future (VFX, sound), then:
        enemy.ReturnToPool();
    }
}