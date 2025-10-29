using UnityEngine;
using DG.Tweening;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject currentScreen;
    private Sequence _screensSequence;
    private const float ScreenWidth = 1920f;
    private const float Duration = 0.8f;

    private void Awake() => ServiceLocator.Register(this);

    public void ShowScreen(GameObject screen)
    {
        if (screen == null || screen == currentScreen) return;

        // make sure incoming screen is active so anchoredPosition is meaningful
        screen.gameObject.SetActive(true);

        // kill previous animation
        _screensSequence?.Kill();

        // pick direction based on anchoredPosition
        var incomingX = screen.GetComponent<RectTransform>().anchoredPosition.x;
        var moveOutTarget = incomingX > 0 ? -ScreenWidth : ScreenWidth;

        _screensSequence = DOTween.Sequence();
        _screensSequence.Append(currentScreen.GetComponent<RectTransform>().DOAnchorPos(new Vector2(moveOutTarget, 0), Duration).SetEase(Ease.OutQuad));
        _screensSequence.Join(screen.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, Duration).SetEase(Ease.OutQuad));
        _screensSequence.OnComplete(() =>
        {
            // hide previous if desired
            currentScreen.gameObject.SetActive(false);
            currentScreen = screen;
        });
    }
}
