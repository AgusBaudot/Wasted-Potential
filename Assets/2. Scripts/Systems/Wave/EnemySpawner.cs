using UnityEngine;

[RequireComponent(typeof(FactoryProvider))]
public class EnemySpawner : MonoBehaviour
{
    private FactoryProvider _factoryProvider;
    private GridManager _grid;
    private EnemyManager _enemyManager;
    private IEnemyFactory _enemyFactory;

    private void Awake()
    {
        _factoryProvider = GetComponent<FactoryProvider>();
        _enemyFactory = _factoryProvider.GetFactory(EnemyType.Bear);
    }

    public EnemyBase Spawn(EnemyType type, Vector2Int gridPos)
    {
        _grid ??= ServiceLocator.Get<GridManager>();
        Vector3 world = _grid.GridToWorld(gridPos);
        return Spawn(type, world);
    }

    public EnemyBase Spawn(EnemyType type, Vector3 worldPos)
    {
        _grid ??= ServiceLocator.Get<GridManager>();
        var factory = _factoryProvider.GetFactory(type);
        var enemy = factory.Create(worldPos, factory);

        _enemyManager ??= ServiceLocator.Get<EnemyManager>();
        _enemyManager.RegisterEnemy(enemy);

        return enemy;
    }

    public void Release(EnemyBase enemy)
    {
        //Spawner-specific cleanup if needed in future (VFX, sound), then:
        enemy.ReturnToPool();
    }
}