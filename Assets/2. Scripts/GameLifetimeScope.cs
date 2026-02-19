using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Dependencies")]

    #region Managers

    [SerializeField] private UpdateManager updateManager;
    [SerializeField] private GridManager gridManager;

    #endregion

    #region Factories
    
    [SerializeField] private BeaverFactory beaverFactory;
    [SerializeField] private DeerFactory deerFactory;
    [SerializeField] private BearFactory bearFactory;
    [SerializeField] private RabbitFactory rabbitFactory;
    [SerializeField] private ForestBeastFactory forestBeastFactory;

    #endregion
    
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(updateManager).As<IUpdateManager>();
        builder.RegisterComponent(gridManager).As<IGridQuery>();

        builder.RegisterComponent(beaverFactory).As<IEnemyFactory>();
        builder.RegisterComponent(deerFactory).As<IEnemyFactory>();
        builder.RegisterComponent(bearFactory).As<IEnemyFactory>();
        builder.RegisterComponent(rabbitFactory).As<IEnemyFactory>();
        builder.RegisterComponent(forestBeastFactory).As<IEnemyFactory>();
    }
}