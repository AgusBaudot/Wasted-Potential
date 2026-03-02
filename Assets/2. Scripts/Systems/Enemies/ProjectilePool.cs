using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ProjectilePool : MonoBehaviour, IProjectilePool
{
    [SerializeField] private Projectile defaultProjectilePrefab;

    private readonly Queue<Projectile> _pool = new();
    private IObjectResolver _container;
    
    [Inject]
    public void Construct(IObjectResolver container)
    {
        _container = container;
    }

    public Projectile Spawn(Vector3 startPos, Projectile prefab = null)
    {
        prefab ??= defaultProjectilePrefab;

        Projectile proj;
        if (_pool.Count > 0)
        {
            proj = _pool.Dequeue();
            proj.gameObject.SetActive(true);
            if (proj.GetComponent<Animator>().runtimeAnimatorController != prefab.GetComponent<Animator>().runtimeAnimatorController)
                proj.GetComponent<Animator>().runtimeAnimatorController = prefab.GetComponent<Animator>().runtimeAnimatorController;
        }
        else
        {
            proj = Instantiate(prefab, startPos, Quaternion.identity);
            _container.InjectGameObject(proj.gameObject);
        }

        return proj;
    }

    public void Release(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        _pool.Enqueue(proj);
    }
}
