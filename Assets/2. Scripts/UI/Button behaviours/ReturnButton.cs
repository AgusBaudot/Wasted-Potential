using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        ServiceLocator.Get<MenuUIManager>().ShowScreen(mainMenuScreen);
    }
}
