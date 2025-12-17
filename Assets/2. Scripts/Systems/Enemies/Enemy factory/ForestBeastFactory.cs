using UnityEngine;
using UnityEngine.Pool;

public class ForestBeastFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private ForestBeast bossPrefab;
    [SerializeField] private EnemyData bossData;
    
    private ObjectPool<EnemyBase> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<EnemyBase>(
            createFunc: () => Instantiate(bossPrefab),
            actionOnGet: (f) => f.gameObject.SetActive(true),
            actionOnRelease: (f) => f.Reset(),
            actionOnDestroy: (f) => Destroy(f.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public EnemyBase Create(Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        var boss = _pool.Get();
        boss.Initialize(bossData, spawnPosition, originFactory);
        return boss;
    }

    public void Release(EnemyBase enemy)
    {
        if (enemy is ForestBeast boss)
            _pool.Release(boss);
        else
            Debug.LogWarning("Tried to release non-ForestBeast to ForestBeastFactory");
    }
}