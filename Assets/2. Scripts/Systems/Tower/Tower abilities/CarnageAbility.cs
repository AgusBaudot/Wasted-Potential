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
        tracker.Initialize(tiers);
    }

    public override void Fire(Tower tower, EnemyBase target)
    {
        if (target.Data.type == EnemyType.Boss)
        OnEnemyHit(tower, target);
        
        // Find the tracker on this specific tower and tell it to update
        var tracker = tower.gameObject.GetComponent<CarnageTracker>();
        if (tracker != null)
        {
            tracker.RegisterHit(target);
        }
    }
}