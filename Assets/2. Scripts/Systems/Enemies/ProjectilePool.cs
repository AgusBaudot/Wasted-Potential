using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile defaultProjectilePrefab;

    private readonly Queue<Projectile> _pool = new();

    private void Awake() => ServiceLocator.Register(this);

    private void OnDestroy() => ServiceLocator.Unregister(this);

    public Projectile Spawn(Vector3 startPos, Projectile prefab = null)
    {
        if (prefab == null) prefab = defaultProjectilePrefab;

        if (_pool.Count > 0)
        {
            var proj = _pool.Dequeue();
            proj.gameObject.SetActive(true);
            return proj;
        }
        var projObj = Instantiate(defaultProjectilePrefab, startPos, Quaternion.identity);
        return projObj;
    }

    public void Release(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        _pool.Enqueue(proj);
    }
}
