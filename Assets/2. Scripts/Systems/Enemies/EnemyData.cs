using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "TD/Enemies/New enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public float moveSpeed;
    public int damageOnReach;
    public Sprite image;
    //Add bounty if reward on kill.
}