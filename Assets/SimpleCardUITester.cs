using UnityEngine;

public class SimpleCardUITester : MonoBehaviour
{
    [SerializeField] private PlayerHand playerHand; // player hand with available cards
    private TowerPlacementFacade _towerPlacementFacade;
    private CardData _selectedCard;

    private void Start()
    {
        _towerPlacementFacade = ServiceLocator.Get<TowerPlacementFacade>();
    }

    private void Update()
    {
        // If a card is selected, handle mouse clicks for placing
        if (_selectedCard != null)
        {
            // ignore clicks over UI (EventSystem) if you have UI present
            if (Input.GetMouseButtonDown(0))
            {
                var cam = Camera.main;
                if (cam == null) return;

                Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f; // ensure pos in tilemap plane
                Vector2Int gridPos = GridManager.Instance.WorldToGrid(mouseWorld);

                // Optional: preview/check
                var tile = GridManager.Instance.GetTile(gridPos);
                if (tile == null || !tile.Buildable)
                {
                    Debug.LogWarning("Selected tile not buildable.");
                }
                else
                {
                    bool success = _towerPlacementFacade.TryPlaceTower(_selectedCard, gridPos);
                    if (success)
                    {
                        Debug.Log($"Placed {_selectedCard.cardName} at {gridPos}");
                        // Optionally remove card from hand here:
                        // playerHand.RemoveCard(_selectedCard);
                        _selectedCard = null; // deselect after place
                    }
                    else
                    {
                        Debug.LogWarning("Placement failed (insufficient resources or other).");
                    }
                }
            }

            // Right click cancels selection
            if (Input.GetMouseButtonDown(1))
                _selectedCard = null;
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 240, 500));
        GUILayout.Label("Player Hand:");

        if (playerHand == null || playerHand.AvailableCards.Count == 0)
        {
            GUILayout.Label("No cards available.");
        }
        else
        {
            foreach (var card in playerHand.AvailableCards)
            {
                string btn = $"{card.cardName} (Cost: {card.cost})";
                if (GUILayout.Button(btn))
                {
                    _selectedCard = card; // pick card to place
                    Debug.Log($"Selected card {_selectedCard.cardName}. Click a tile to place (right-click to cancel).");
                }
            }
        }

        // show currently selected
        if (_selectedCard != null)
        {
            GUILayout.Space(8);
            GUILayout.Label($"Selected: {_selectedCard.cardName} (click map to place)");
            if (GUILayout.Button("Cancel"))
                _selectedCard = null;
        }

        GUILayout.EndArea();
    }
}
