using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Oil Slow")]
public class OilAbility : TowerAbility
{
    public OilLifeTime oilPrefab; //Prefab with sprite renderer & OilLifeTime component
    [Tooltip("Speed will be base/factor")]
    public float slowFactor = 0.5f;
    public float lifeTime = 5f;

    public override bool OnEnemyHit(Tower tower, EnemyBase enemy)
    {
        var oilGO = Instantiate(oilPrefab, enemy.transform.position, Quaternion.identity);
        oilGO.Init(lifeTime);
        oilGO.OnLifeTimeExpired += HandleOnLifeTimeExpired;
        enemy.ApplySlow(slowFactor, lifeTime);
        return true;
    }

    private void HandleOnLifeTimeExpired()
    {
        //Additional logic when oil lifetime expires can be added here.
    }
}