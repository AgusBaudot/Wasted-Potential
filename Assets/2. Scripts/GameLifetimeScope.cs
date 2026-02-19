using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Dependencies")]

    #region Managers

    [SerializeField] private UpdateManager updateManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private TowerManager towerManager;

    #endregion

    #region Factories
    
    [SerializeField] private TowerFactory towerFactory;
    [SerializeField] private FactoryProvider factoryProvider;

    #endregion

    #region Low-level

    [SerializeField] private TowerInfoUI towerInfoUI;
    [SerializeField] private EnemySpawner enemySpawner;

    #endregion
    
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(updateManager).As<IUpdateManager>();
        builder.RegisterComponent(gridManager).As<IGridQuery>();
        builder.RegisterComponent(enemyManager).As<IEnemyQuery>();
        builder.RegisterComponent(towerManager).As<ITowerRegistry>();

        builder.RegisterComponent(towerFactory).As<ITowerFactory>();
        builder.RegisterComponent(factoryProvider).AsSelf();

        builder.RegisterComponent(towerInfoUI).AsSelf();
        builder.RegisterComponent(enemySpawner).AsSelf();
    }
}