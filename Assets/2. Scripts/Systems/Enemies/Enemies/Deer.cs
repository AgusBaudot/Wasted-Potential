using UnityEngine;

public class Deer : EnemyBase
{
    private bool _ability;
    
    public override void ApplyDamage(float amount, GameObject source)
    {
        //Apply damage normally
        base.ApplyDamage(amount, source);
        
        //After that, check remaining HP for ability
        if (Health.Max / 4 > Health.Current && _ability)
        {
            _baseMoveSpeed = 0.5f;
            _ability = false;
        }
    }
}