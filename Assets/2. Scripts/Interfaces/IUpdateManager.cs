public interface IUpdateManager
{
    void Register(IUpdatable updatable);
    void Unregister(IUpdatable updatable);
}