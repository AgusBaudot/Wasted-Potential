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

        var projObj = Instantiate(defaultProjectilePrefab, startPos, Quaternion.identity);
        return projObj;
    }

    public void Release(Projectile proj)
    {
        Destroy(proj.gameObject);
        Debug.LogWarning("Make actual pool");
    }
}
