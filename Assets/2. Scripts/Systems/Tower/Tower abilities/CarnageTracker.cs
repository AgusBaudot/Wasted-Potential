using System;
using UnityEngine;
using VContainer;

public class CarnageTracker : MonoBehaviour
{
    private int _enemyCount;
    private int _oilCount;
    private Sprite[] _tiers;
    private SpriteRenderer _spriteRenderer;
    private IWaveQuery _waveManager;
    private IResourcesQuery _resourceManager;

    [Inject]
    public void Construct(IWaveQuery waveManager, IResourcesQuery resourceManager)
    {
        _waveManager = waveManager ?? throw new ArgumentNullException(nameof(waveManager));
        _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
    }

    public void Initialize(Sprite[] tierSprites)
    {
        _tiers = tierSprites;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _waveManager.OnWaveCompleted += HandleOnWaveCompleted;
    }

    public void RegisterHit(EnemyBase target)
    {
        if (_enemyCount >= 3) return; // Cap at 3

        if (target.StatusManager.Has("Slow"))
            _oilCount++;

        _enemyCount++;

        if (_spriteRenderer != null && _enemyCount < _tiers.Length)
        {
            _spriteRenderer.sprite = _tiers[_enemyCount];
        }
    }

    private void HandleOnWaveCompleted(int idx)
    {
        if (_enemyCount >= 3)
        {
            _resourceManager.GainResources(5 + System.Math.Max(0, _oilCount * 2 - 1));
        }

        if (_spriteRenderer != null) _spriteRenderer.sprite = _tiers[0];
        _enemyCount = 0;
        _oilCount = 0;
    }
    
    public bool CanRegisterHit() => _enemyCount < 3;

    private void OnDestroy()
    {
        _waveManager.OnWaveCompleted -= HandleOnWaveCompleted;
    }
}