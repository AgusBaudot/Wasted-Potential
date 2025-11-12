using TMPro;
using UnityEngine;

public class PlayingUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _wavesText;
    [SerializeField] private TextMeshProUGUI _resourcesText;

    private void Start()
    {
        ServiceLocator.Get<WaveManager>().OnWaveStarted += HandleWaveStarted;
        ServiceLocator.Get<ResourceManager>().OnResourcesChanged += HandleResourcesChanged;
        ServiceLocator.Get<PlayerHealthManager>().OnHealthChanged += HandleHealthChanged;
    }

    private void HandleResourcesChanged(int amount)
    {
        _resourcesText.text = $"Gear: {amount}";
    }

    private void HandleWaveStarted(int index)
    {
        _wavesText.text = $"Wave #{index + 1}";
    }

    private void HandleHealthChanged(int amount)
    {
        _healthText.text = $"Health: {amount}";
    }
}
