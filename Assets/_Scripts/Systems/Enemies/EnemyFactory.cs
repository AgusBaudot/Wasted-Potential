using UnityEngine;
using UnityEngine.Pool;

public class EnemyFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private Enemy enemyPrefab;
    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(enemyPrefab),
            actionOnGet: (e) => e.gameObject.SetActive(true),
            actionOnRelease: (e) => e.Reset(),
            actionOnDestroy: (e) => Destroy(e.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public Enemy Create(Vector3 spawnPosition)
    {
        var enemy = _pool.Get();
        enemy.Initialize(spawnPosition);
        return enemy;
    }

    public void Release(Enemy enemy) => _pool.Release(enemy);
}