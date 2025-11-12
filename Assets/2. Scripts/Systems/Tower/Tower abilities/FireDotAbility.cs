using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Fire DOT")]
public class FireDotAbility : TowerAbility
{
    public float dotDuration = 3f;
    public int dotDamagePerSecond = 3;

    public override bool OnEnemyHit(Tower tower, EnemyBase enemy)
    {
        enemy.ApplyDot(dotDamagePerSecond, dotDuration);
        return true;
    }
}