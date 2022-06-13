public class PCAbilityUsing : IPlayerControllerState
{
    public override void Do()
    {
        throw new System.NotImplementedException();
    }

    public override void Entry()
    {
        AttachedContainer.DisableMovement();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override PlayerController.State GetStateType()
    {
        throw new System.NotImplementedException();
    }

    public override IPlayerControllerState HandleEvent(PlayerController.Event pEvent)
    {
        throw new System.NotImplementedException();
    }
}