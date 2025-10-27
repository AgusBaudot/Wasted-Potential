using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<CardData, GameObject> OnClicked;
    public bool Selected => _selected;

    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardPriceText;
    [SerializeField] private TextMeshProUGUI cardRarityText;
    [SerializeField] private Image cardImage;

    private bool _mouseOver = false;
    private bool _selected = false;
    private CardData data;
    private Vector2 _originalPosition = Vector2.zero;
    private Vector2 _destinePosition = new Vector2(1000, 75);

    public void Init(CardData data)
    {
        this.data = data;
        UISetup();
    }

    private void UISetup()
    {
        cardNameText.text = data.cardName;
        cardPriceText.text = data.cost.ToString();
        cardRarityText.text = data.rarity.ToString();
        cardImage.sprite = data.image;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_mouseOver || _selected)
            return;
        _mouseOver = true;

        GetComponent<RectTransform>().DOAnchorPosY(50f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_mouseOver || _selected)
            return;
        _mouseOver = false;

        GetComponent<RectTransform>().DOAnchorPosY(-50f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnClicked?.Invoke(data, gameObject);
    }

    public void MoveCard()
    {
        _mouseOver = false;
        var rectTransform = GetComponent<RectTransform>();
        if (!_selected)
        {
            _originalPosition = rectTransform.anchoredPosition;
            _originalPosition.y = -50;
            rectTransform.DOAnchorPos(_destinePosition, 0.3f).SetEase(Ease.OutQuad);
            _selected = true;
        }
        else
        {
            rectTransform.DOAnchorPos(_originalPosition, 0.3f).SetEase(Ease.OutQuad);
            _originalPosition = Vector2.zero;
            _selected = false;
        }
    }
}