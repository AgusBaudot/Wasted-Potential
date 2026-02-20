using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class CardPlacementController : MonoBehaviour, IUpdatable
{
    [SerializeField] private CardManager _cardManager;
    [SerializeField] private TextMeshProUGUI _resourcesText;
    
    private IGridQuery _gridManager;
    private IUpdateManager _updateManager;
    private CardVisualizer _cardVisualizer;
    private PlayerHand _playerHand;
    private TowerPlacementFacade _placementFacade;
    private CardData _selectedCard;
    private GameObject _ghostInstance;

    [Inject]
    public void Construct(IGridQuery gridManager, IUpdateManager updateManager)
    {
        _gridManager = gridManager ?? throw new ArgumentNullException(nameof(gridManager));
        _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
    }

    private void Start()
    {
        _cardVisualizer = _cardManager.CardVisualizer;
        _playerHand = _cardManager.PlayerHand;
        
        _cardVisualizer.OnCardSelected += HandleCardSelected;
        _cardVisualizer.OnCardDeselected += HandleCardDeselected;
        
        _placementFacade = ServiceLocator.Get<TowerPlacementFacade>();
        _updateManager.Register(this);
    }

    private void OnDestroy()
    {
        if (_cardVisualizer != null)
        {
            _cardVisualizer.OnCardSelected -= HandleCardSelected;
            _cardVisualizer.OnCardDeselected -= HandleCardDeselected;
        }

        _updateManager.Unregister(this);
        DestroyGhost();
    }

    private void HandleCardSelected(CardData card = null)
    {
        if (card == null) 
            CancelSelection();
        
        _selectedCard = card;
        CreateGhost();
    }

    public void Tick(float deltaTime)
    {
        if (_selectedCard == null)
            return;

        //Cancel on right click or escape
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSelection();
            return;
        }
        
        //Ignore clicks over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        
        //Update ghost to mouse snapped tile
        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0))
            TryPlaceAtMouse();
    }

    private void UpdateGhostPosition()
    {
        if (_ghostInstance == null)
            return;

        Vector3 mouseWorld = Helpers.GetCamera().ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        var gridPos = _gridManager.WorldToGrid(mouseWorld);
        var tile = _gridManager.GetTile(gridPos);
        _ghostInstance.transform.position = _gridManager.GridToWorld(gridPos);
        
        //Tiny red if over not buildable
        var sr = _ghostInstance.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = (tile != null && tile.Buildable)
                ? _selectedCard.ghostPrefab.GetComponent<SpriteRenderer>().color
                : new Color(1, 0, 0, 0.5f);
    }

    private void TryPlaceAtMouse()
    {
        Vector3 mouseWorld = Helpers.GetCamera().ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2Int gridPos = _gridManager.WorldToGrid(mouseWorld);
        
        var tile = _gridManager.GetTile(gridPos);
        if (tile == null || !tile.Buildable)
        {
            // Show not placeable?
            return;
        }

        bool success = _placementFacade.TryPlaceTower(_selectedCard, gridPos);
        if (success)
        {
            //Ref do something
            CancelSelection();
        }
        else
        {
            //If fail was because of insufficient resources.
            if (!ServiceLocator.Get<ResourceManager>().CanAfford(_selectedCard.cost))
            {
                CancelSelection();
                if (_resourcesText.TryGetComponent<UITextShake>(out var shaker))
                    shaker.Shake();
                Debug.LogError("Play SFX");
            }
        }
    }

    private void CreateGhost()
    {
        DestroyGhost();
        
        _ghostInstance = Instantiate(_selectedCard.ghostPrefab);
        _ghostInstance.SetActive(true);
        UpdateGhostPosition();
    }

    private void DestroyGhost()
    {
        if (_ghostInstance != null)
            Destroy(_ghostInstance);
        
        _ghostInstance = null;
    }

    private void HandleCardDeselected() => CancelSelection();

    private void CancelSelection()
    {
        _selectedCard = null;
        _cardVisualizer.DeselectCard();
        DestroyGhost();
    }
}
