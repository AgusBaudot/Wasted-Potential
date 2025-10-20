using UnityEngine;

//Facade that runs the tower build command
public class TowerPlacementFacade : MonoBehaviour
{
    [SerializeField] private MonoBehaviour towerFactoryComponent;
    private ITowerFactory _towerFactory;
    
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
        GridTile tile = GridManager.Instance.GetTile(gridPosition);
        if (tile == null || !tile.Buildable) return false;
        
        if (!ServiceLocator.Get<ResourceManager>().CanAfford(card.cost)) return false;
        
        //If all checks pass, create and execute the command.
        ICommand buildCommand = new BuildTowerCommand(card, gridPosition,  _towerFactory);
        return buildCommand.Execute();
    }
}

//Inject dependencies into BuildTowerCommand (pass resourcemanager, gridmanager and a tower factory).
//Command should update tile state (set buildable = false) so that other towers aren't built upon it.
//Use factory/pool for towers and use DI