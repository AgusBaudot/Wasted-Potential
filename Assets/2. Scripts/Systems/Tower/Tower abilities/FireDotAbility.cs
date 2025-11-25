using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Fire DOT")]
public class FireDotAbility : ProjectileAttack
{
    public float dotDuration = 3f;
    public int dotDamagePerSecond = 3;

    public override void OnEnemyHit(Tower tower, EnemyBase enemy)
    {
        enemy.ApplyDot(dotDamagePerSecond, dotDuration);
    }
}