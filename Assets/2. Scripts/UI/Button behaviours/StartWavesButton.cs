using UnityEngine;
using UnityEngine.UI;

public class StartWavesButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
    }

    private void HandleOnClick()
    {
        ServiceLocator.Get<WaveManager>().StartWaves();
        gameObject.SetActive(false);
    }
}
