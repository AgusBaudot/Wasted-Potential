using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameStateManager _stateManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _stateManager = new GameStateManager();

        ServiceLocator.Register<IGameStateService>(_stateManager);
        ServiceLocator.Register<IGameStateController>(_stateManager);

        _stateManager.OnStateChanged += HandleStateChanged;
    }

    private void Start()
    {
        _stateManager.ChangeState(new MenuState(_stateManager));
    }

    private void HandleStateChanged(IState newState)
    {
        Debug.Log($"[GameManager] State changed to: {newState.GetType().Name}");
    }
}
