using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartWavesButton : MonoBehaviour
{
    private bool _hasStarted = false;
    private WaveManager _waveManager;

    private RectTransform _rectTransform;
    private Vector2 _startPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPos = _rectTransform.anchoredPosition;
        
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
        _waveManager = ServiceLocator.Get<WaveManager>();
        _waveManager.OnNewCardOffer += () => StartCoroutine(HandleActivation());
        //Check if coroutine starts. If not, call it from here.
        StartCoroutine(HandleActivation());
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

    private IEnumerator HandleActivation()
    {
        gameObject.SetActive(true);
        yield return Helpers.GetWait(3);
        StartMoving();
    }

    private void StartMoving()
    {
        // 1. The Vertical Bobbing (The Core Float)
        // We move the anchor Y position up by floatDistance
        _rectTransform.DOAnchorPosY(_startPos.y + 15, 1)
            .SetEase(Ease.InOutSine) // Crucial for the "weightless" feel
            .SetLoops(-1, LoopType.Yoyo); // -1 means infinite, Yoyo means Up->Down->Up

        // 2. The Breathing Effect (Optional Polish)
        // Slight scaling makes the object feel "alive"
        transform.DOScale(Vector3.one * 1.05f, 1)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}