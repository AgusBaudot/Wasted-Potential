using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IUpdatable, IPoolable
{
    //Enemy has to fire event when end is reached or dies.
    public event Action<Enemy> OnRemoved;

    private GridManager _grid;
    private GridTile currentTile;

    private Vector3 _spawnPosition;
    private Vector3 _targetPos;
    private UpdateManager _updateManager;
    private IEnemyFactory _originFactory;

    public void Initialize(Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        _spawnPosition = spawnPosition;
        _originFactory = originFactory;
        transform.position = _spawnPosition;
        
        _grid = GridManager.Instance;
        currentTile = _grid.GetTile(_grid.WorldToGrid(spawnPosition));
        _targetPos = _grid.GetTile(currentTile.Next).Center;

        _updateManager ??= ServiceLocator.Get<UpdateManager>();
        _updateManager.Register(this);
    }

    public void Tick(float deltaTime)
    {
        if (currentTile == null || currentTile.Type == GridTileType.Goal)
        {
            OnRemoved?.Invoke(this);
            return;
        }

        if (Vector3.Distance(transform.position, _targetPos) < 0.1f)
        {
            transform.position = _targetPos;

            if (currentTile.Type == GridTileType.Goal)
            {
                OnRemoved?.Invoke(this);
                return;
            }

            var nextPos = currentTile.Next;
            var nextTile = _grid.GetTile(nextPos);

            if (nextTile == null)
            {
                OnRemoved?.Invoke(this);
                return;
            }

            currentTile = nextTile; // Update tile based on logic, not position
            _targetPos = nextTile.Center;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, deltaTime * 3);
    }

    public void Reset()
    {
        _updateManager.Unregister(this);
        transform.position = _spawnPosition;
        gameObject.SetActive(false);
    }

    public void ReturnToPool()
    {
        _originFactory?.Release(this);
    }

    public void Die() => OnRemoved?.Invoke(this);
}