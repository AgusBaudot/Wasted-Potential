using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartWavesButton : MonoBehaviour
{
    private bool _hasStarted = false;
    private WaveManager _waveManager;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
        _waveManager = ServiceLocator.Get<WaveManager>();
        _waveManager.OnNewCardOffer += () => gameObject.SetActive(true);
    }

    private void HandleOnClick()
    {
        if (!_hasStarted)
        {
            _waveManager.StartWaves();
            GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
            _hasStarted = true;
            gameObject.SetActive(false);
        }
        else
        {
            _waveManager.ConfirmNextWave();
            gameObject.SetActive(false);
        }
    }
}