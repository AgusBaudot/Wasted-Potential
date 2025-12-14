using UnityEngine;

public interface ITargetable
{
    Vector3 WorldPosition { get; }

    bool IsAlive { get; }

    void ApplyDamage(float amount, GameObject source);
}