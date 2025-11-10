using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IUpdatable, IPoolable, ITargetable
{
    //Enemy has to fire event when end is reached or dies.
    public event Action<Enemy> OnRemoved;
    public int Health => _health;

    private GridManager _grid;
    private GridTile currentTile;

    private Vector3 _spawnPosition;
    private Vector3 _targetPos;
    private UpdateManager _updateManager;
    private IEnemyFactory _originFactory;
    private int _health;
    private int _maxHealth = 100;

    public Vector3 WorldPosition => transform.position;

    public bool IsAlive => _health > 0;

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

        _health = _maxHealth;
    }

    public void Tick(float deltaTime)
    {
        if (currentTile == null || currentTile.Type == GridTileType.Goal)
        {
            ReachedEnd();
            return;
        }

        if (Vector3.Distance(transform.position, _targetPos) < 0.1f)
        {
            transform.position = _targetPos;

            if (currentTile.Type == GridTileType.Goal)
            {
                ReachedEnd();
                return;
            }

            var nextPos = currentTile.Next;
            var nextTile = _grid.GetTile(nextPos);

            if (nextTile == null)
            {
                ReachedEnd();
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

    public void ApplyDamage(int amount, GameObject source)
    {
        _health -= amount;
        if (_health <= 0)
            Die();
    }

    private void ReachedEnd()
    {
        ServiceLocator.Get<HealthManager>().ApplyDamage(5);
        OnRemoved?.Invoke(this);
    }
}