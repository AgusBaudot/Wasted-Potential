using System;
using UnityEngine;
using VContainer;

public class WaveRewards : MonoBehaviour
{
    private IWaveQuery _waveManager;
    private IResourcesQuery _resourceManager;

    [Inject]
    public void Construct(IWaveQuery waveManager, IResourcesQuery resourceManager)
    {
        _waveManager = waveManager ?? throw new ArgumentNullException(nameof(waveManager));
        _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
    }
    
    private void Start()
    {
        _waveManager.OnWaveCompleted += HandleWaveCompleted;
    }

    private void HandleWaveCompleted(int waveIndex)
    {
        _resourceManager.GainResources(10 + (waveIndex + 1) / 3 * 5);
    }
}
