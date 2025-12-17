using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class CardDisplay : MonoBehaviour
{
    public event Action<CardData, GameObject> OnClicked;
    public bool Selected => _selected;

    public bool IsInteractive { get; set; } = true;
    
    public CardData Data => data;

    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
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
        cardDescriptionText.text = data.description;
        cardPriceText.text = data.cost.ToString();
        cardRarityText.text = data.rarity.ToString();
        cardImage.sprite = data.image;
        
        switch (data.rarity)
        {
            case CardRarity.Common:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/Common card");
                break;
            case CardRarity.Rare:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/Rare card");
                break;
            case CardRarity.Epic:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/Epic card");
                break;
        }
    }

    public void HandlePointerEnter(PointerEventData eventData)
    {
        if (_mouseOver || _selected) return;
        if (!IsInteractive) return;
        
        _mouseOver = true;
        GetComponent<RectTransform>().DOAnchorPosY(50f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void HandlePointerExit(PointerEventData eventData)
    {
        if (!_mouseOver || _selected) return;
        if (!IsInteractive) return;
        
        _mouseOver = false;
        GetComponent<RectTransform>().DOAnchorPosY(-50f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void HandlePointerClick(PointerEventData eventData)
    {
        if (!IsInteractive) return;
        
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