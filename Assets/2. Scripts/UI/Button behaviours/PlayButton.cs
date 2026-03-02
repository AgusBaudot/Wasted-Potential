using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectorScreen;
    [SerializeField] private GameObject level1;
    
    private IMenuUIManager _menuUIManager;

    [Inject]
    public void Construct(IMenuUIManager menuUIManager)
    {
        _menuUIManager = menuUIManager ?? throw new ArgumentNullException(nameof(menuUIManager));
    }
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        _menuUIManager.ShowScreen(levelSelectorScreen);
        EventSystem.current.SetSelectedGameObject(level1);
    }
}
