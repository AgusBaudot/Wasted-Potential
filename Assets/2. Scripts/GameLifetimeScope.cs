using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Dependencies")]
    [SerializeField] private UpdateManager updateManager;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(updateManager).As<IUpdateManager>();
    }
}