using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;
    private GridManager _grid;

    public Enemy Spawn(Vector2Int gridPos)
    {
        _grid??= GridManager.Instance;
        Vector3 world = _grid.GridToWorld(gridPos);
        return Spawn(world);
    }

    public Enemy Spawn(Vector3 worldPos)
    {
        _grid??= GridManager.Instance;
        var enemy = _enemyFactory.Create(worldPos);
        return enemy;
    }

    public void Release(Enemy enemy)
    {
        //Spawner-specific cleanup if needed in future (VFX, sound), then:
        _enemyFactory.Release(enemy);
    }
}