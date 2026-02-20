using UnityEngine;
using VContainer;

//Facade that runs the tower build command
public class TowerPlacementFacade : MonoBehaviour
{
    [SerializeField] private MonoBehaviour towerFactoryComponent;
    private ITowerFactory _towerFactory;
    private IGridQuery _gridManager;

    [Inject]
    public void Construct(IGridQuery gridQuery)
    {
        _gridManager = gridQuery;
    }
    
    private void Awake()
    {
        ServiceLocator.Register(this);
        
        _towerFactory = towerFactoryComponent as ITowerFactory;
        if (_towerFactory == null) Debug.LogWarning("Tower Factory not assigned or does not implement ITowerFactory");
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    public bool TryPlaceTower(CardData card, Vector2Int gridPosition)
    {
        //Check all conditions.
        GridTile tile = _gridManager.GetTile(gridPosition);
        if (tile == null || !tile.Buildable) return false;
        
        if (!ServiceLocator.Get<ResourceManager>().CanAfford(card.cost)) return false;
        
        //If all checks pass, create and execute the command.
        ICommand buildCommand = new BuildTowerCommand(card, gridPosition,  _towerFactory, _gridManager);
        return buildCommand.Execute();
    }
}