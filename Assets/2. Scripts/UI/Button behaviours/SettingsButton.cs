using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject settingsScreen;
    
    private IMenuUIManager _menuUIManager;

    [Inject]
    public void Construct(IMenuUIManager menuUIManager)
    {
        _menuUIManager = menuUIManager ?? throw new NullReferenceException(nameof(menuUIManager));
    }
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        _menuUIManager.ShowScreen(settingsScreen);
    }
}
