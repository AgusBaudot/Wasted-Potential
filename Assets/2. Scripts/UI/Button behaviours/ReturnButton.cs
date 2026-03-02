using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ReturnButton : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;

    private IMenuUIManager _menuUIManager;

    [Inject]
    public void Construct(IMenuUIManager menuUIManager)
    {
        _menuUIManager = menuUIManager ?? throw new NullReferenceException(nameof(menuUIManager));
        Debug.Log($"[ReturnButton] Injected on {gameObject.name}");
    }
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        Debug.Log($"[ReturnButton] Clicked on {gameObject.name}, menuUIManager null: {_menuUIManager == null}");
        _menuUIManager.ShowScreen(mainMenuScreen);
    }
}
