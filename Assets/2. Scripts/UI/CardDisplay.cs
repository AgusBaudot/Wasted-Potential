using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardPriceText;
    [SerializeField] private TextMeshProUGUI cardRarityText;
    [SerializeField] private Image cardImage;
    
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
}
