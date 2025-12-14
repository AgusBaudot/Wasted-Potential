using System;
using System.Collections.Generic;

public class EnemyStatusManager
{
    //Active by id
    private readonly Dictionary<string, StatusInstance> _active = new();
    private readonly EnemyBase _owner;

    public EnemyStatusManager(EnemyBase owner)
    {
        _owner = owner;
    }

    public void Tick(float dt)
    {
        if (_active.Count == 0) return;
        
        //Collect removals to avoid modifying dictionary during iteration
        var toRemove = new List<string>();
        
        foreach (var kvp in _active)
        {
            var id = kvp.Key;
            var inst = kvp.Value;
            
            //1. Substract time
            inst.remaining -= dt;
            
            //2. Handle ticks
            if (inst.definition.tickInterval > 0)
            {
                inst.tickAccumulator += dt;
                
                while (inst.tickAccumulator >= inst.definition.tickInterval)
                {
                    inst.tickAccumulator -= inst.definition.tickInterval;
                    inst.definition.OnTick(_owner, inst, inst.definition.tickInterval);
                    
                    //If tick killed enemy, stop immediately.
                    if (!_owner.IsAlive) return;
                }
            }

            //3. Cleanup
            if (inst.remaining <= 0)
            {
                //Schedule remove.
                toRemove.Add(id);
            }
        }

        foreach (var id in toRemove)
            RemoveStatusInternal(id, StatusRemoveReason.Expired);
    }

    public bool Has(string id) => _active.ContainsKey(id);

    public StatusInstance Get(string id)
    {
        _active.TryGetValue(id, out var inst);
        return inst;
    }
    
    //Apply status effect to enemy
    public void Apply(StatusEffect def)
    {
        if (def == null) return;
        
        //Handle reactions: check if any existing active status triggers reaction with 'def'
        foreach (var kvp in new Dictionary<string, StatusInstance>(_active))
        {
            var inst = kvp.Value;
            if (inst.definition.reactions == null) continue;

            foreach (var r in inst.definition.reactions)
            {
                if (r.otherId == def.id)
                {
                    //Reaction found: spawn resultEffect if any
                    if (r.resultEffect != null)
                        Apply(r.resultEffect);
                    
                    if (r.removeSelf) Remove(inst.definition.id);
                    //Treat the incoming def as consumed, don't apply.
                    if (r.removeOther) return;
                }
            }
        }

        if (_active.TryGetValue(def.id, out var existing))
        {
            switch (def.onReapply)
            {
                case OnApplyBehaviour.Replace:
                    RemoveStatusInternal(def.id, StatusRemoveReason.Replaced);
                    CreateAndApply(def);
                    break;
                
                case OnApplyBehaviour.Stack:
                    existing.stacks++;
                    existing.remaining = MathF.Max(existing.remaining, def.baseDuration);
                    def.OnApply(_owner, existing); //Allow Status effect to react to stacking.
                    break;
                
                case  OnApplyBehaviour.RefreshDuration:
                default:
                    existing.Refresh();
                    def.OnApply(_owner, existing);
                    break;
            }
        }

        else
        {
            CreateAndApply(def);
        }
    }

    private void CreateAndApply(StatusEffect def)
    {
        var isnt = new StatusInstance(def);
        _active[def.id] = isnt;
        def.OnApply(_owner,  isnt);
    }

    public void Remove(string id)
    {
        if (_active.ContainsKey(id)) RemoveStatusInternal(id, StatusRemoveReason.Forced);
    }
    
    public enum StatusRemoveReason { Expired, Forced, Replaced }

    private void RemoveStatusInternal(string id, StatusRemoveReason reason)
    {
        if (!_active.TryGetValue(id, out var inst)) return;
        inst.definition.OnRemove(_owner, inst);
        _active.Remove(id);
    }

    public void ClearAll()
    {
        foreach (var kvp in new List<KeyValuePair<string, StatusInstance>>(_active))
            RemoveStatusInternal(kvp.Key, StatusRemoveReason.Forced);
    }

    #region Debuffs

    public float GetSpeedMultiplier()
    {
        float totalMult = 1;
        foreach (var kvp in _active)
        {
            //Multiply all active slow effects
            totalMult *= kvp.Value.definition.speedMultiplier;
        }

        return totalMult;
    }

    public bool IsStunned()
    {
        foreach (var kvp in _active)
        {
            if (kvp.Value.definition.isStun)
                return true;
        }
        return false;
    }

    #endregion
}