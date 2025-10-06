using UnityEngine;
using UnityEngine.Pool;

public class BearFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private Bear bearPrefab;
    private ObjectPool<Bear> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Bear>(
            createFunc: () => Instantiate(bearPrefab),
            actionOnGet: (b) => b.gameObject.SetActive(true),
            actionOnRelease: (b) => b.Reset(),
            actionOnDestroy: (b) => Destroy(b.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public Enemy Create(Vector3 spawnPosition, IEnemyFactory  originFactory = null)
    {
        var bear = _pool.Get();
        bear.Initialize(spawnPosition, originFactory);
        return bear;
    }

    public void Release(Enemy enemy)
    {
        if (enemy is Bear bear)
            _pool.Release(bear);
        else
            Debug.LogWarning("Tried to release non-bear to BearFactory");
    }
}