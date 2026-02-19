using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

public class RabbitFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private Rabbit rabbitPrefab;
    [SerializeField] private EnemyData rabbitData;
    
    private ObjectPool<EnemyBase> _pool;

    [Inject]
    public void Construct(IObjectResolver container)
    {
        _pool = new ObjectPool<EnemyBase>(
            createFunc: () =>
            {
                var instance = Instantiate(rabbitPrefab);
                container.InjectGameObject(instance.gameObject);
                return instance;
            },
            actionOnGet: (r) => r.gameObject.SetActive(true),
            actionOnRelease: (r) => r.Reset(),
            actionOnDestroy: (r) => Destroy(r.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public EnemyBase Create(Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        var rabbit = _pool.Get();
        rabbit.Initialize(rabbitData, spawnPosition, originFactory);
        return rabbit;
    }

    public void Release(EnemyBase enemy)
    {
        if (enemy is Rabbit rabbit)
            _pool.Release(rabbit);
        else
            Debug.LogWarning("Tried to release non-rabbit to RabbitFactory");
    }
}