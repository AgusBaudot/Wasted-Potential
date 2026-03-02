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
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private PlayingUIManager playingUIManager;
    [SerializeField] private HealthBarManager healthBarManager;

    #endregion

    #region Factories & Pools
    
    [SerializeField] private TowerFactory towerFactory;
    [SerializeField] private FactoryProvider factoryProvider;
    [SerializeField] private ProjectilePool projectilePool;

    #endregion

    #region Low-level

    [SerializeField] private TowerInfoUI towerInfoUI;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CardPlacementController cardPlacementController;
    [SerializeField] private TowerPlacementFacade towerPlacementFacade;
    [SerializeField] private WaveRewards waveRewards;
    [SerializeField] private StartWavesButton startWavesButton;
    [SerializeField] private ChoiceCardBehavior choiceCardBehavior;
    [SerializeField] private InitialCardsContinueButton initialCardsContinueButton;

    #endregion
    
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(updateManager).As<IUpdateManager>();
        builder.RegisterComponent(gridManager).As<IGridQuery>();
        builder.RegisterComponent(enemyManager).As<IEnemyQuery>();
        builder.RegisterComponent(towerManager).As<ITowerRegistry>();
        builder.RegisterComponent(waveManager).As<IWaveQuery>();
        builder.RegisterComponent(cardManager).As<ICardManager>();
        builder.RegisterComponent(resourceManager).As<IResourcesQuery>();
        builder.RegisterComponent(playingUIManager).AsSelf();
        builder.RegisterComponent(healthBarManager).As<IHealthBarManager>();

        builder.RegisterComponent(towerFactory).As<ITowerFactory>();
        builder.RegisterComponent(factoryProvider).AsSelf();
        builder.RegisterComponent(projectilePool).As<IProjectilePool>();

        builder.RegisterComponent(towerInfoUI).AsSelf();
        builder.RegisterComponent(enemySpawner).AsSelf();
        builder.RegisterComponent(cardPlacementController).AsSelf();
        builder.RegisterComponent(towerPlacementFacade).AsSelf();
        builder.RegisterComponent(waveRewards).AsSelf();
        builder.RegisterComponent(startWavesButton).AsSelf();
        builder.RegisterComponent(choiceCardBehavior).AsSelf();
        builder.RegisterComponent(initialCardsContinueButton).AsSelf();

        builder.Register<BuildTowerCommand>(Lifetime.Singleton);
    }
}