using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
    private readonly List<IUpdatable> _toAdd = new List<IUpdatable>();
    private readonly List<IUpdatable> _toRemove = new List<IUpdatable>();

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    public void Register(IUpdatable updatable)
    {
        if (!_toAdd.Contains(updatable))
            _toAdd.Add(updatable);
    }

    public void Unregister(IUpdatable updatable)
    {
        if (!_toRemove.Contains(updatable))
            _toRemove.Add(updatable);
    }

    private void Update()
    {
        foreach (var u in _toRemove)
            _updatables.Remove(u);
        _toRemove.Clear();

        foreach (var u in _toAdd)
            _updatables.Add(u);
        _toAdd.Clear();
        
        foreach (IUpdatable updatable in _updatables)
            updatable.Tick(Time.deltaTime);
    }
}