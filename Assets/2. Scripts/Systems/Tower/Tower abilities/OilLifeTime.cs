using System;
using UnityEngine;
using VContainer;

public class OilLifeTime : MonoBehaviour, IUpdatable
{
    public event Action OnLifeTimeExpired;

    [SerializeField] private float _lifeTime = 2f;

    private float _currentTime;
    private IUpdateManager _updateManager;

    [Inject]
    public void Construct(IUpdateManager updateManager)
    {
        _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
    }

    public void Init(float lifeTime)
    {
        _lifeTime = lifeTime;
        _updateManager.Register(this);
    }

    public void Tick(float deltaTime)
    {
        _currentTime += deltaTime;
        if (_currentTime >= _lifeTime)
        {
            _updateManager.Unregister(this);
            OnLifeTimeExpired?.Invoke();
            Destroy(gameObject);
        }
    }
}