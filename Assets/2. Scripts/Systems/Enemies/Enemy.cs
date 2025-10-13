using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IUpdatable, IPoolable
{
    //Enemy has to fire event when end is reached or dies.
    public event Action<Enemy> OnRemoved;

    private GridManager _grid;
    private GridTile currentTile;

    private Vector3 _spawnPosition;
    private UpdateManager _updateManager;
    private GridTile _goalTile;
    private IEnemyFactory _originFactory;

    public void Initialize(Vector3 spawnPosition, IEnemyFactory originFactory = null)
    {
        _spawnPosition = spawnPosition;
        _originFactory = originFactory;
        transform.position = _spawnPosition;
        
        _grid = GridManager.Instance;
        currentTile = _grid.GetTile(_grid.WorldToGrid(spawnPosition));

        _goalTile = _grid.GoalTile;
        _updateManager ??= ServiceLocator.Get<UpdateManager>();
        _updateManager.Register(this);
    }

    public void Tick(float deltaTime)
    {
        currentTile = _grid.GetTile(_grid.WorldToGrid(transform.position)); //Recalculate current tile.

        if (Vector3.Distance(transform.position, _grid.GridToWorld(_goalTile.GridPosition)) < 0.1f || currentTile == null || currentTile.Next == null)
        {
            OnRemoved?.Invoke(this);
            return;
        }

        Vector3 targetPos = _grid.GridToWorld(currentTile.Next);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, deltaTime * 3);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            transform.position = targetPos;
        }
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