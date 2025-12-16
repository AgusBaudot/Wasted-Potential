using System;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Towers/Ability/Carnage")]
public class CarnageAbility : InstantAttack
{
    //Subscribe to OnWaveCompleteed form WaveManager and check for all "end-wave" things.
    private int _enemyCount;
    private int _oilCount;
    private ResourceManager _resourceManager;

    public override void OnPlaced(Tower tower)
    {
        ServiceLocator.Get<WaveManager>().OnWaveCompleted += HandleOnWaveCompleted;
        base.OnPlaced(tower);
    }

    public override void Fire(Tower tower, EnemyBase target)
    {
        Debug.Log(_enemyCount);
        if (_enemyCount == 3) return;
        
        OnEnemyHit(tower, target);
        Debug.Log("actual attack");
        if (target.StatusManager.Has("Slow"))
            _oilCount++;
        _enemyCount++;
    }

    private void HandleOnWaveCompleted(int idx)
    {
        if (_enemyCount == 3)
        {
            _resourceManager ??= ServiceLocator.Get<ResourceManager>();
            _resourceManager.GainResources(5 + Math.Max(0, _oilCount * 2 - 1)); //Has 3 enemies
            //Give visual feedback.
            //Put cash-in sound?
        }

        _enemyCount = 0;
        _oilCount = 0;
    }
}