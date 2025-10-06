using UnityEngine;
using UnityEngine.Pool;

public class RabbitFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private Rabbit rabbitPrefab;
    private ObjectPool<Rabbit> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Rabbit>(
            createFunc: () => Instantiate(rabbitPrefab),
            actionOnGet: (r) => r.gameObject.SetActive(true),
            actionOnRelease: (r) => r.Reset(),
            actionOnDestroy: (r) => Destroy(r.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public Enemy Create(Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        var rabbit = _pool.Get();
        rabbit.Initialize(spawnPosition, originFactory);
        return rabbit;
    }

    public void Release(Enemy enemy)
    {
        if (enemy is Rabbit rabbit)
            _pool.Release(rabbit);
        else
            Debug.LogWarning("Tried to release non-rabbit to RabbitFactory");
    }
}