using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardDisplay))]
public class CardInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private CardDisplay _display;

    private void Awake() => _display = GetComponent<CardDisplay>();
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_display.IsInteractive) return;
        _display.HandlePointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_display.IsInteractive) return;
        _display.HandlePointerExit(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_display.IsInteractive) return;
        _display.HandlePointerClick(eventData);
    }
}