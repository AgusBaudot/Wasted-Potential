using System;
using TMPro;
using UnityEngine;
using VContainer;

public class PlayingUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _wavesText;
    [SerializeField] private TextMeshProUGUI _resourcesText;

    private IWaveQuery _waveManager;
    private IResourcesQuery _resourceManager;
    private IPlayerHealthManager _playerHealthManager;

    [Inject]
    public void Construct(IWaveQuery waveManager, IResourcesQuery resourceManager, IPlayerHealthManager playerHealthManager)
    {
        _waveManager = waveManager ?? throw new ArgumentNullException(nameof(waveManager));
        _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        _playerHealthManager = playerHealthManager ?? throw new ArgumentNullException(nameof(playerHealthManager));
    }

    private void Start()
    {
        _waveManager.OnWaveStarted += HandleWaveStarted;
        _resourceManager.OnResourcesChanged += HandleResourcesChanged;
        _playerHealthManager.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleResourcesChanged(int amount)
    {
        _resourcesText.text = amount.ToString();
    }

    private void HandleWaveStarted(int index)
    {
        _wavesText.text = (index + 1).ToString();
    }

    private void HandleHealthChanged(int amount)
    {
        _healthText.text = amount.ToString();
    }
}
