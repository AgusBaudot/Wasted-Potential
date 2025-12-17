using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Spawn On Hit")]
public class SpawnOnHitAbility : ProjectileAttack
{
    [Header("Visuals")]
    public OilLifeTime oilPrefab; //Prefab with sprite renderer & OilLifeTime component
    public float lifeTime = 5f;

    public override void OnEnemyHit(Tower tower, EnemyBase enemy)
    {
        //1. Visual Logic (Specific to this ability)
        var go = Instantiate(oilPrefab, enemy.transform.position, Quaternion.identity);
        go.Init(lifeTime);
        
        //2. Damage (standard)
        enemy.ApplyDamage(tower.Data.damage, tower.gameObject);
        
        //3. Status logic (Generic)
        if (tower.Data.onHitStatus != null)
            enemy.ApplyStatus(tower.Data.onHitStatus);
    }
}