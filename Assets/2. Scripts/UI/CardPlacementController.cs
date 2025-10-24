using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlacementController : MonoBehaviour, IUpdatable
{
    [SerializeField] private CardManager _cardManager;
    
    private CardVisualizer _cardVisualizer;
    private PlayerHand _playerHand;
    private TowerPlacementFacade _placementFacade;
    private CardData _selectedCard;
    private GameObject _ghostInstance;

    private void Start()
    {
        _cardVisualizer = _cardManager.CardVisualizer;
        _playerHand = _cardManager.PlayerHand;
        
        _cardVisualizer.OnCardSelected += HandleCardSelected;
        
        _placementFacade = ServiceLocator.Get<TowerPlacementFacade>();
        ServiceLocator.Get<UpdateManager>().Register(this);
    }

    private void OnDestroy()
    {
        if (_cardVisualizer != null)
            _cardVisualizer.OnCardSelected += HandleCardSelected;

        ServiceLocator.Get<UpdateManager>().Unregister(this);
        DestroyGhost();
    }

    private void HandleCardSelected(CardData card)
    {
        var resourceManager = ServiceLocator.Get<ResourceManager>();
        if (resourceManager != null && !resourceManager.CanAfford(card.cost))
            return;
        
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
        var gridPos = GridManager.Instance.WorldToGrid(mouseWorld);
        var tile = GridManager.Instance.GetTile(gridPos);
        _ghostInstance.transform.position = GridManager.Instance.GridToWorld(gridPos);
        
        //Tiny red if over not buildable
        var sr = _ghostInstance.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            sr.color = (tile != null && tile.Buildable) ? new Color(255, 255, 255, 64) : new Color(255, 0, 0, 64);
    }

    private void TryPlaceAtMouse()
    {
        Vector3 mouseWorld = Helpers.GetCamera().ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2Int gridPos = GridManager.Instance.WorldToGrid(mouseWorld);
        
        var tile = GridManager.Instance.GetTile(gridPos);
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
            //failed (affordability or other), keep selection active or notify player.
            //Shake resources text?
        }
    }

    private void CreateGhost()
    {
        DestroyGhost();
        
        Vector3 mouseWorld = Helpers.GetCamera().ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        var gridPos = GridManager.Instance.WorldToGrid(mouseWorld);
        
        _ghostInstance = Instantiate(_selectedCard.ghostPrefab, GridManager.Instance.GridToWorld(gridPos), Quaternion.identity);
        _ghostInstance.SetActive(true);
    }

    private void DestroyGhost()
    {
        if (_ghostInstance != null)
            Destroy(_ghostInstance);
        
        _ghostInstance = null;
    }

    private void CancelSelection()
    {
        _selectedCard = null;
        DestroyGhost();
    }
}
