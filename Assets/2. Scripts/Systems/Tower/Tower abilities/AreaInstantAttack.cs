using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Area Instant Attack")]
public class AreaInstantAttack : InstantAttack
{
    public override void Fire(Tower tower, EnemyBase target)
    {
        tower.gameObject.GetComponent<ShockwaveAnimation>().SetTower(tower);
    }
}