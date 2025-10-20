using UnityEngine;

public enum CardRarity
{
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "New card", menuName = "Cards/Card data")]
public class CardData : ScriptableObject
{
    [Header("Card Info")] 
    public string cardName;
    public CardRarity rarity;
    public int cost;
    [TextArea] public string description;
    
    [Header("Tower")]
    public GameObject towerPrefab;

    [Header("Tower Info")]
    public int damage;
    public float range;
    public float fireRate;
}