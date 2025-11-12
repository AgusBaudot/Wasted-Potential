using UnityEngine;
using UnityEngine.Pool;

public class BeaverFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private Beaver beaverPrefab;
    [SerializeField] private EnemyData beaverData;
    
    private ObjectPool<EnemyBase> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<EnemyBase>(
            createFunc: () => Instantiate(beaverPrefab),
            actionOnGet: (b) => b.gameObject.SetActive(true),
            actionOnRelease: (b) => b.Reset(),
            actionOnDestroy: (b) => Destroy(b.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public EnemyBase Create(Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        var beaver = _pool.Get();
        beaver.Initialize(beaverData, spawnPosition, originFactory);
        return beaver;
    }

    public void Release(EnemyBase enemy)
    {
        if (enemy is Beaver beaver)
            _pool.Release(beaver);
        else
            Debug.LogWarning("Tried to release non-beaver to beaverFactory");
    }
}