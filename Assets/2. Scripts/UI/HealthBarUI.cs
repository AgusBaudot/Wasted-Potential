using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Vector2 _worldOffset = new Vector2(0f, 1.5f);
    
    private Slider _slider;
    private IHasHealth _bound;
    private Transform _targetTransform;
    private RectTransform _rectTransform;

    public void Bind(IHasHealth source, Transform target)
    {
        _slider ??= GetComponent<Slider>();
        _rectTransform ??= GetComponent<RectTransform>();
        
        Unbind();
        _bound = source;
        _targetTransform = target;
        
        _slider.maxValue = _bound.Max;
        _slider.value = _bound.Current;
        
        source.OnHealthChanged += HandleOnHealthChanged;
        source.OnDeath += HandleOnDeath;
        
        gameObject.SetActive(true);
    }

    private void HandleOnHealthChanged(int current, int max) => _slider.value = current;
    private void HandleOnDeath() => gameObject.SetActive(false);

    public void Unbind()
    {
        if (_bound == null) return;
        
        _targetTransform = null;
        _bound.OnHealthChanged -= HandleOnHealthChanged;
        _bound.OnDeath -= HandleOnDeath;
        _bound = null;
        gameObject.SetActive(false);
    }

    public void UpdatePosition(Camera cam, RectTransform canvasRect)
    {
        if (_targetTransform == null || canvasRect == null) return;

        Vector3 worldPos = _targetTransform.position + (Vector3)_worldOffset;
        Vector3 viewport = cam.WorldToViewportPoint(worldPos);
        
        //Later (and optional):
        //Cull if off-camera or behind it.

        if (!gameObject.activeSelf) gameObject.SetActive(true);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            null,
            out localPoint);
        
        _rectTransform.anchoredPosition = localPoint;
    }
    
    private void OnDisable() => Unbind();
}