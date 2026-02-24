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
    // Resource & player health managers.

    [Inject]
    public void Construct(IWaveQuery waveManager)
    {
        _waveManager = waveManager ?? throw new ArgumentNullException(nameof(waveManager));
    }

    private void Start()
    {
        _waveManager.OnWaveStarted += HandleWaveStarted;
        ServiceLocator.Get<ResourceManager>().OnResourcesChanged += HandleResourcesChanged;
        ServiceLocator.Get<PlayerHealthManager>().OnHealthChanged += HandleHealthChanged;
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
