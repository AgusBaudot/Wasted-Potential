using UnityEngine;

public class WaveRewards : MonoBehaviour
{
    private WaveManager _waveManager;
    private ResourceManager _resourceManager;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _waveManager = ServiceLocator.Get<WaveManager>();

        _waveManager.OnWaveCompleted += HandleWaveCompleted;
    }

    private void HandleWaveCompleted(int waveIndex)
    {
        _resourceManager.GainResources(10 + (waveIndex + 1) / 3 * 5);
    }
}
