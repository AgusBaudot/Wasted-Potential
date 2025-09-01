using UnityEngine;

public interface IPoolable
{
    void Initialize(Vector3 spawnPosition);
    void Reset();
}