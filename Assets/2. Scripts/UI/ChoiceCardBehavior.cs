using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class ChoiceCardBehavior : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _selectedSound;
    
    private AudioSource _source;
    private ICardManager _cardManager;

    [Inject]
    public void Construct(ICardManager cardManager)
    {
        _cardManager = cardManager ?? throw new ArgumentNullException(nameof(cardManager));
    }
    
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
    }

    private void HandleOnClick()
    {
        _source.PlayOneShot(_selectedSound);
        _cardManager.FinalizeCardChoice(GetComponent<CardDisplay>().Data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _source.PlayOneShot(_hoverSound);
    }
}