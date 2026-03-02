using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] private GameManager gameManager;

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(gameManager).AsSelf();
        
        builder.Register<GameStateManager>(Lifetime.Singleton)
            .As<IGameStateController>()
            .As<IGameStateService>();
    }
}