using UnityEngine;

//Tower placing with command implementation.
public class BuildTowerCommand : ICommand
{
    private readonly CardData _card;
    private readonly Vector2Int _gridPosition;
    private ResourceManager _resourceManager;
    private readonly ITowerFactory _factory;

    public BuildTowerCommand(CardData card, Vector2Int gridPosition, ITowerFactory factory)
    {
        _card = card;
        _gridPosition = gridPosition;
        _factory = factory;

        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }
    
    public bool Execute()
    {
        if (!_resourceManager.TrySpend(_card.cost))
            return false;
        
        Vector3 worldPos = GridManager.Instance.GridToWorld(_gridPosition);
        if (!_factory.TryCreate(_card, worldPos, _gridPosition, out Tower tower))
        {
            //if failed to create, refund resources.
            _resourceManager.GainResources(_card.cost);
            return false;
        }

        var tile = GridManager.Instance.GetTile(_gridPosition);
        if (tile != null)
            tile.SetBuildable(false);
        
        return true;
    }
}