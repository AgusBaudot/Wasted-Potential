using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Carnage")]
public class CarnageAbility : InstantAttack
{
    [SerializeField] private Sprite[] tiers;

    public override void OnPlaced(Tower tower)
    {
        base.OnPlaced(tower);
        
        // Add the tracker to the specific Tower instance
        var tracker = tower.gameObject.AddComponent<CarnageTracker>();
        tower.Container.Inject(tracker);
        tracker.Initialize(tiers);
    }

    public override void Fire(Tower tower, EnemyBase target)
    {
        if (target.Data.type == EnemyType.Boss)
            return;

        var tracker = tower.gameObject.GetComponent<CarnageTracker>();
        if (tracker == null) return;

        // Check cap before doing anything
        if (!tracker.CanRegisterHit()) return;

        OnEnemyHit(tower, target);  // insta-kills via ApplyDamage
        tracker.RegisterHit(target);
    }
}