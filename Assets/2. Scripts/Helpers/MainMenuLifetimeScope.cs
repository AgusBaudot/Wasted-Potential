using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private MenuUIManager menuUIManager;
    [SerializeField] private ReturnButton returnButtonLevelSelector;
    [SerializeField] private ReturnButton returnButtonOptions;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(menuUIManager).As<IMenuUIManager>();
        builder.RegisterComponentInHierarchy<PlayButton>();
        builder.RegisterComponentInHierarchy<SettingsButton>();
        builder.RegisterComponentInHierarchy<LevelSelectedButton>();
    }

    protected override void Awake()
    {
        base.Awake(); // builds the container
        Container.Inject(returnButtonLevelSelector);
        Container.Inject(returnButtonOptions);
    }
}
