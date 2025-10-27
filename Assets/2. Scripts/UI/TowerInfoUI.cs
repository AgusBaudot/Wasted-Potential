using System;
using TMPro;
using UnityEngine;

public class TowerInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject tooltipUI;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI fireRateText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float padding = 10;

    private bool _isActive;
    private RectTransform _tooltipRect;
    private RectTransform _canvasRect;
    private float _fadeTime = 0.1f;
    private RectTransform _panel;

    private void Start()
    {
        _tooltipRect = tooltipUI.GetComponent<RectTransform>();
        var canvas = tooltipUI.GetComponentInParent<Canvas>();
        _canvasRect = canvas.GetComponent<RectTransform>();

        var manager = ServiceLocator.Get<TowerManager>();
        foreach (var tower in manager.AllTowers)
        {
            tower.OnMouseEnterTower += Show;
            tower.OnMouseExitTower += Hide;
        }
        
        manager.OnTowerAdded += HandleOnTowerAdded;

        _panel = _tooltipRect.gameObject.transform.GetChild(0) as RectTransform;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        //Convert screen point to local point in canvas space.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect,
            Input.mousePosition,
            null,
            out Vector2 localPoint);
        
        //Horizontal bounds
        float rightEdge = localPoint.x + (1 - _panel.pivot.x) * _panel.rect.size.x;
        float leftEdge = localPoint.x - _panel.pivot.x * _panel.rect.size.x;

        //Flip to the left
        if (rightEdge > _canvasRect.rect.size.x / 2 - padding)
            localPoint.x -= _panel.rect.size.x;
        //Flip to the right
        else if (leftEdge < -_canvasRect.rect.size.x / 2 + padding)
            localPoint.x += _panel.rect.size.x;
        
        float topEdge = localPoint.y + (1 - _panel.pivot.y) * _panel.rect.size.y;
        float bottomEdge = localPoint.y - _panel.pivot.y * _panel.rect.size.y;
        
        //Move down
        if (topEdge > _canvasRect.rect.size.y / 2 - padding)
            localPoint.y -= _panel.rect.size.y * 0.5f;
        //Move up
        else if (bottomEdge < -_canvasRect.rect.size.y / 2 + padding)
            localPoint.y += _panel.rect.size.y * 0.5f;

        _tooltipRect.anchoredPosition = localPoint;

        Debug.Log(
            $"Mouse: {Input.mousePosition} || Local:{localPoint} || Panelsize: {_panel.rect.size} || Canvas half: {_canvasRect.sizeDelta} || Pivot: {_panel.pivot} || Right: {rightEdge} || Left: {leftEdge} || Top: {topEdge} || Bottom: {bottomEdge}");
    }

    private void OnDestroy()
    {
        var manager = ServiceLocator.Get<TowerManager>();
        foreach (var tower in manager.AllTowers)
        {
            tower.OnMouseEnterTower -= Show;
            tower.OnMouseExitTower -= Hide;
        }

        manager.OnTowerAdded -= HandleOnTowerAdded;
    }

    private void Show(Tower tower)
    {
        Helpers.FadeIn(tooltipUI.GetComponent<CanvasGroup>(), _fadeTime, 0.5f);
        nameText.text = tower.Data.cardName;
        descriptionText.text = tower.Data.description;
        costText.text = tower.Data.cost.ToString();
        rarityText.text = tower.Data.rarity.ToString();
        damageText.text = tower.Data.damage.ToString();
        rangeText.text = tower.Data.range.ToString();
        fireRateText.text = tower.Data.fireRate.ToString();
        _isActive = true;
    }

    private void Hide(Tower tower)
    {
        Helpers.FadeOut(tooltipUI.GetComponent<CanvasGroup>(), _fadeTime);
        _isActive = false;
    }

    private void HandleOnTowerAdded(Tower tower)
    {
        tower.OnMouseEnterTower += Show;
        tower.OnMouseExitTower += Hide;
    }
}