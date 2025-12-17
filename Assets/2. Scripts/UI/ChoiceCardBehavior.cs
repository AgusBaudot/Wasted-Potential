using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceCardBehavior : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _selectedSound;
    
    private AudioSource _source;
    
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
    }

    private void HandleOnClick()
    {
        _source.PlayOneShot(_selectedSound);
        ServiceLocator.Get<CardManager>().FinalizeCardChoice(GetComponent<CardDisplay>().Data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _source.PlayOneShot(_hoverSound);
    }
}