using System;
using UnityEngine;

public class OilLifeTime : MonoBehaviour, IUpdatable
{
    public event Action OnLifeTimeExpired;

    [SerializeField] private float _lifeTime = 2f;

    private float _currentTime;

    public void Init(float lifeTime)
    {
        _lifeTime = lifeTime;
        ServiceLocator.Get<UpdateManager>().Register(this);
    }

    public void Tick(float deltaTime)
    {
        _currentTime += deltaTime;
        if (_currentTime >= _lifeTime)
        {
            ServiceLocator.Get<UpdateManager>().Unregister(this);
            OnLifeTimeExpired?.Invoke();
            Destroy(gameObject);
        }
    }
}