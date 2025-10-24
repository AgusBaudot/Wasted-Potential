using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<CardData> OnClicked;
    
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardPriceText;
    [SerializeField] private TextMeshProUGUI cardRarityText;
    [SerializeField] private Image cardImage;

    private bool _mouseOver = false;
    
    private CardData data;
    
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
        if (_mouseOver) 
            return;
        _mouseOver = true;

        GetComponent<RectTransform>().DOAnchorPosY(50f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_mouseOver) 
            return;
        _mouseOver = false;

        GetComponent<RectTransform>().DOAnchorPosY(-50f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnClicked?.Invoke(data);
    }
}
