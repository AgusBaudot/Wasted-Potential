using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject settingsScreen;
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        //Should activate settings panel.
        ServiceLocator.Get<MenuUIManager>().ShowScreen(settingsScreen);
    }
}
