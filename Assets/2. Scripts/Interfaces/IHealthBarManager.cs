using UnityEngine;

public interface IHealthBarManager
{
    void Register(IHasHealth source, Transform target);
    void Unregister(IHasHealth source);
    void Tick(float deltaTime);
}