using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IUpdatable, IPoolable
{
    //Enemy has to fire event when end is reached or dies.
    public event Action<Enemy> OnRemoved;

    private Vector3 _spawnPosition;
    private UpdateManager _updateManager;
    private GridTile _goalTile;

    public void Initialize(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
        transform.position = _spawnPosition;
        
        _goalTile = GridManager.Instance.GoalTile;
        _updateManager ??= ServiceLocator.Get<UpdateManager>();
        _updateManager.Register(this);
    }

    public void Tick(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, GridManager.Instance.GridToWorld(_goalTile.GridPosition), deltaTime * 3);

        if (Vector3.Distance(transform.position, GridManager.Instance.GridToWorld(_goalTile.GridPosition)) < 0.1f)
        {
            OnRemoved?.Invoke(this);
        }
    }

    public void Reset()
    {
        _updateManager.Unregister(this);
        transform.position = _spawnPosition;
        gameObject.SetActive(false);
    }

    public void Die() => OnRemoved?.Invoke(this);
}