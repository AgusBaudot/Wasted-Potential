using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Dependencies")]
    [SerializeField] private UpdateManager updateManager;
    [SerializeField] private GridManager gridManager;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(updateManager).As<IUpdateManager>();
        builder.RegisterComponent(gridManager).As<IGridQuery>();
    }
}