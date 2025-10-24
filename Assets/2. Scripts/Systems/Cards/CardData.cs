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
    public Sprite image;
    [TextArea] public string description;
    
    [Header("Tower")]
    public GameObject towerPrefab;
    public GameObject ghostPrefab;

    [Header("Tower Info")]
    public int damage;
    public float range;
    public float fireRate;
}