using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectorScreen;
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        ServiceLocator.Get<MenuUIManager>().ShowScreen(levelSelectorScreen);
    }
}
