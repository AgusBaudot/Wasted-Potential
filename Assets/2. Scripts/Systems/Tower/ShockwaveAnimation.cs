using UnityEngine;

public class ShockwaveAnimation : MonoBehaviour
{
    private Animator _anim;
    private Tower _tower;
    
    public void SetTower(Tower tower) => _tower = tower;
    
    public void PlayShockWave()
    {
        _anim ??= transform.GetChild(0).GetComponent<Animator>();
        _anim.SetTrigger("Shockwave");
        
        Collider2D [] hits = Physics2D.OverlapBoxAll(_tower.transform.position, Vector2.one * _tower.Data.range, 0);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyBase enemy))
            {
                if (enemy is ITargetable t)
                {
                    t.ApplyDamage(_tower.Data.damage, _tower.gameObject);
                }
                
                if (_tower.Data.onHitStatus != null)
                {
                    enemy.ApplyStatus(_tower.Data.onHitStatus);
                }
            }
        }
    }
}
