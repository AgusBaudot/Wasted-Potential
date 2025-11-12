using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour, IUpdatable
{
    [SerializeField] private HealthBarUI healthBarPrefab;
    [SerializeField] private RectTransform canvasRect;

    private Camera _cam;
    private readonly Queue<HealthBarUI> _pool = new();
    private List<HealthBarUI> _active = new();
    private Dictionary<IHasHealth, HealthBarUI> _map = new();

    private void Awake() => ServiceLocator.Register(this);
    private void Start() => ServiceLocator.Get<UpdateManager>().Register(this);

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
        ServiceLocator.Get<UpdateManager>().Unregister(this);
    }

    public void Register(IHasHealth source, Transform target)
    {
        if (source == null || target == null || _map.ContainsKey(source))
            return;
        
        HealthBarUI bar = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(healthBarPrefab, canvasRect);
        bar.gameObject.SetActive(true);
        bar.Bind(source, target);
        _active.Add(bar);
        _map[source] = bar;

        source.OnDeath += () => Unregister(source);
    }

    public void Unregister(IHasHealth source)
    {
        if (source == null || !_map.TryGetValue(source, out var bar)) return;
        
        source.OnDeath -= () => Unregister(source);
        _map.Remove(source);
        _active.Remove(bar);
        bar.Unbind();
        _pool.Enqueue(bar);
    }

    public void Tick(float deltaTime)
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var bar = _active[i];
            if (bar == null)
            {
                _active.RemoveAt(i);
                continue;
            }

            _cam ??= Helpers.GetCamera();
            bar.UpdatePosition(_cam, canvasRect);
        }
    }
}
