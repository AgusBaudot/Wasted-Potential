using UnityEngine;
using UnityEngine.UI;

public class ChoiceCardBehavior : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
    }

    private void HandleOnClick()
    {
        ServiceLocator.Get<CardManager>().FinalizeCardChoice(GetComponent<CardDisplay>().Data);
    }
}