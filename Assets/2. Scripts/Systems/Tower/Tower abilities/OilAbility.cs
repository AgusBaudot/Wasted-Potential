using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Oil Slow")]
public class OilAbility : TowerAbility
{
    public OilLifeTime oilPrefab; //Prefab with sprite renderer & OilLifeTime component
    public float lifeTime = 5f;

    public override bool OnEnemyHit(Tower tower, EnemyBase enemy)
    {
        var oilGO = Instantiate(oilPrefab, enemy.transform.position, Quaternion.identity);
        oilGO.Init(lifeTime);
        oilGO.OnLifeTimeExpired += HandleOnLifeTimeExpired;

        return true;
    }

    private void HandleOnLifeTimeExpired()
    {
        //Additional logic when oil lifetime expires can be added here.
    }
}