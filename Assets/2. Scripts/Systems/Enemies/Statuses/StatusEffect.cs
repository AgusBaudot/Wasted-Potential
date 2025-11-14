using UnityEngine;

public enum OnApplyBehaviour { Replce, Stack, RefreshDuration}

[CreateAssetMenu(menuName = "TD/New status")]
public class StatusEffect : ScriptableObject
{
    public string id; //Unique id: "Burn", "Slow", etc.
    public float baseDuration;
    public float tickInterval; //0 = no periodic tick.
    public OnApplyBehaviour onReapply = OnApplyBehaviour.RefreshDuration;
    public bool isExclusive = false; //If true, only one instance allowed.

    //[Header("Reaction rules"), Tooltip("If this effect meets other 'id', spawn 'resultEffect'")]
    //public ReactionRule[] reactions;

    //[System.Serializable]
    //public struct 
}