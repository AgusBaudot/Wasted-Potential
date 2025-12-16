using UnityEngine;
using DG.Tweening;

public class UITextShake : MonoBehaviour
{
    private RectTransform _rect;
    private Vector2 _originalPos;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _originalPos = _rect.anchoredPosition;
    }

    public void Shake()
    {
        _rect.DOKill();
        _rect.anchoredPosition = _originalPos;
        _rect.DOShakeAnchorPos(
            duration: 0.25f,
            strength: new Vector2(15, 0),
            vibrato: 20,
            randomness: 90);
    }
}