using UnityEngine;

public interface IProjectilePool
{
    Projectile Spawn(Vector3 startPos, Projectile prefab = null);
    void Release(Projectile proj);
}