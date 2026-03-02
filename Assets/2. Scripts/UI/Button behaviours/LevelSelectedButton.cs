using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using VContainer;

public class LevelSelectedButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private int level;
    
    private Vector2 _size;
    private IGameStateController _stateController;

    [Inject]
    public void Construct(IGameStateController controller)
    {
        _stateController = controller;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
        _size = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    private void HandleOnClick()
    {
        _stateController.ChangeState(new PlayingState(level, _stateController));
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log($"Selected level {level}");
        gameObject.GetComponent<RectTransform>().DOSizeDelta(_size + Vector2.one * 50, 0.5f);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().DOSizeDelta(_size, 0.5f);
    }
}
