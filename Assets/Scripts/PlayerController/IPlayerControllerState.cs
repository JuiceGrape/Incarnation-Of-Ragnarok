public abstract class IPlayerControllerState
{
    protected PlayerController AttachedContainer;

    public abstract IPlayerControllerState HandleEvent(PlayerController.Event pEvent);
    public abstract PlayerController.State GetStateType();
    public abstract void Entry();
    public abstract void Do();
    public abstract void Exit();

    public virtual void Initialize(PlayerController container)
    {
        AttachedContainer = container;
    }
}
