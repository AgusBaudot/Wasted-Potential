using UnityEngine;
using UnityEngine.UI;

public class InitialCardsContinueButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
    }

    private void HandleOnClick()
    {
        ServiceLocator.Get<CardManager>().CardVisualizer.ConfirmInitialCards();
    }
}