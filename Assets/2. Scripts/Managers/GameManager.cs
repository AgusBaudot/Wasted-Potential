using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    private IGameStateService _stateService;
    private IGameStateController _stateController;

    [Inject]
    public void Construct(IGameStateService service, IGameStateController controller)
    {
        _stateService = service;
        _stateController = controller;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _stateService.OnStateChanged += HandleStateChanged;
    }

    private void Start()
    {
        _stateController.ChangeState(new MenuState(_stateController));
    }

    private void HandleStateChanged(IState newState)
    {
        Debug.Log($"[GameManager] State changed to: {newState.GetType().Name}");
    }
}
