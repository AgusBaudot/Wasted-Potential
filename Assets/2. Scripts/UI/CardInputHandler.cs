using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardDisplay))]
public class CardInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _selectedSound;
    
    private CardDisplay _display;
    private AudioSource _source;

    private void Awake()
    {
        _display = GetComponent<CardDisplay>();
        _source = GetComponent<AudioSource>();
    }
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_display.IsInteractive) return;
        _source.PlayOneShot(_hoverSound);
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
        _source.PlayOneShot(_selectedSound);
        _display.HandlePointerClick(eventData);
    }
}