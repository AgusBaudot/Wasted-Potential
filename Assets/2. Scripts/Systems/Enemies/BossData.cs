using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "TD/Enemies/New Boss")]
public class BossData : EnemyData
{
    [Header("Boss Specifics")]
    public float rageSpeedMultiplier = 2;
    public float phaseSwitchHealthPercentage = 0.5f; //50% HP
    public GameObject minionPrefab;
    //Add audio clips for phases, special ability cooldowns, etc.
}