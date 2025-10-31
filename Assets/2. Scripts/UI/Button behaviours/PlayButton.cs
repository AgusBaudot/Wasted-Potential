using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectorScreen;
    [SerializeField] private GameObject level1;
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        ServiceLocator.Get<MenuUIManager>().ShowScreen(levelSelectorScreen);
        EventSystem.current.SetSelectedGameObject(level1);
    }
}
