using UnityEngine;
using UnityEngine.Pool;

public class DeerFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private Deer deerPrefab;
    [SerializeField] private EnemyData deerData;
    
    private ObjectPool<EnemyBase> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<EnemyBase>(
            createFunc: () => Instantiate(deerPrefab),
            actionOnGet: (d) => d.gameObject.SetActive(true),
            actionOnRelease: (d) => d.Reset(),
            actionOnDestroy: (d) => Destroy(d.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public EnemyBase Create(Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        var deer = _pool.Get();
        deer.Initialize(deerData, spawnPosition, originFactory);
        return deer;
    }

    public void Release(EnemyBase enemy)
    {
        if (enemy is Deer deer)
            _pool.Release(deer);
        else
            Debug.LogWarning("Tried to release non-deer to DeerFactory");
    }
}