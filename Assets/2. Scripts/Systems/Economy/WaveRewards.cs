using System;
using UnityEngine;
using VContainer;

public class WaveRewards : MonoBehaviour
{
    private IWaveQuery _waveManager;
    private ResourceManager _resourceManager;

    [Inject]
    public void Construct(IWaveQuery waveManager)
    {
        _waveManager = waveManager ?? throw new ArgumentNullException(nameof(waveManager));
    }
    
    private void Start()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();

        _waveManager.OnWaveCompleted += HandleWaveCompleted;
    }

    private void HandleWaveCompleted(int waveIndex)
    {
        _resourceManager.GainResources(10 + (waveIndex + 1) / 3 * 5);
    }
}
