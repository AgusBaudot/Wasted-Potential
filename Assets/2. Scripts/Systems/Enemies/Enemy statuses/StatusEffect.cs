using UnityEngine;

public enum OnApplyBehaviour { Replace, Stack, RefreshDuration}

[CreateAssetMenu(menuName = "TD/New status")]
public class StatusEffect : ScriptableObject
{
    [Header("Identity")]
    public string id; //Unique id: "Burn", "Slow", etc.
    public float baseDuration; //Set to 0 for instant-only effects (like explosion, or instant cc)
    public bool isExclusive = false; //If true, only one instance allowed.
    public OnApplyBehaviour onReapply = OnApplyBehaviour.RefreshDuration;
    
    [Header("Stats & CC")]
    [Range(0, 2)] public float speedMultiplier = 1;
    public bool isStun = false;
    
    [Header("Damage Logic")]
    public float instantDamage = 0; //Applied immediately on hit/reaction.
    public float damagePerTick = 0; //Applied every interval.
    public float tickInterval = 0; //0 = no ticking.

    [Header("Reaction rules"), Tooltip("If this effect meets other 'id', spawn 'resultEffect'")]
    public ReactionRule[] reactions;

    [System.Serializable]
    public struct ReactionRule
    {
        public string otherId;
        public StatusEffect resultEffect; //can be null, then just remove/replace
        public bool removeSelf;
        public bool removeOther;
    }
    
    // --- LOGIC IMPLEMENTATION ---
    
    //Called when the effect is first applied, refreshed, or stacked.
    public virtual void OnApply(EnemyBase enemy, StatusInstance instance)
    {
        if (instantDamage > 0)
            enemy.ApplyDamage(instantDamage, null);
    }

    //Called automatically by the Manager based on tickInterval
    public virtual void OnTick(EnemyBase enemy, StatusInstance instance, float dt)
    {
        if (damagePerTick > 0)
        {
            //If stacking increases DOT damage, calculate it here.
            float finalDamage = damagePerTick * instance.stacks;
            enemy.ApplyDamage(finalDamage, null);
        }
    }
    
    public virtual void OnRemove(EnemyBase enemy, StatusInstance instance) {}
}