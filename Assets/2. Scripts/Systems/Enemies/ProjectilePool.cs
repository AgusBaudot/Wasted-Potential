using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile defaultProjectilePrefab;

    private readonly Queue<Projectile> _pool = new();

    private void Awake() => ServiceLocator.Register(this);

    private void OnDestroy() => ServiceLocator.Unregister(this);

    public Projectile Spawn(Vector3 startPos, Projectile prefab = null)
        //THIS IS CAUSING THE CONFUSED PROJECTILES: compare both animator controllers so that the returned projectile has the same one as the requested one.
    {
        prefab ??= defaultProjectilePrefab;

        if (_pool.Count > 0)
        {
            var proj = _pool.Dequeue();
            proj.gameObject.SetActive(true);
            if (proj.GetComponent<Animator>().runtimeAnimatorController != prefab.GetComponent<Animator>().runtimeAnimatorController)
                proj.GetComponent<Animator>().runtimeAnimatorController = prefab.GetComponent<Animator>().runtimeAnimatorController;
            return proj;
        }
        var projObj = Instantiate(prefab, startPos, Quaternion.identity);
        return projObj;
    }

    public void Release(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        _pool.Enqueue(proj);
    }
}
