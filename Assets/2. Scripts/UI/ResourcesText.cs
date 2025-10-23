using TMPro;
using UnityEngine;

public class ResourcesText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private ResourceManager _resourceManager;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();

        _resourceManager.OnResourcesChanged += UpdateUI;
        UpdateUI(_resourceManager.CurrentResources);
    }

    private void UpdateUI(int currentResources)
    {
        _text.text = $"Resources: {currentResources}";
    }
}