using UnityEngine;

public interface ITargetable
{
    Vector3 WorldPosition { get; }

    bool IsAlive { get; }

    void ApplyDamage(int amount, GameObject source);
}