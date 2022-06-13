public class PCIdle : IPlayerControllerState
{

    public override void Do()
    {
        //Empty
    }

    public override void Entry()
    {
        AttachedContainer.DisableMovement();
    }

    public override void Exit()
    {
        //Empty
    }

    public override PlayerController.State GetStateType()
    {
        throw new System.NotImplementedException();
    }

    public override IPlayerControllerState HandleEvent(PlayerController.Event pEvent)
    {
        IPlayerControllerState NewState = this;
        switch (pEvent)
        {
            case PlayerController.Event.GroundClicked:
                NewState = new PCMovingToLocation();
                NewState.Initialize(AttachedContainer);
                break;
            case PlayerController.Event.TargetClicked:
                //TODO: Move to target state
                break;
            default:
                //Unhandled event
                break;
        }

        return NewState;
    }
}
