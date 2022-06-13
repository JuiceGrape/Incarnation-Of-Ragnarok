using UnityEngine;

public class PCMovingToLocation : IPlayerControllerState, IDestinationCalculator
{
    private Vector3 CachedDestination;
    public override void Do()
    {
        //Empty
    }

    public override void Entry()
    {
        CachedDestination = AttachedContainer.GetMouseGroundPosition();

        if (CachedDestination != Vector3.negativeInfinity)
        {
            AttachedContainer.SetDestination(CachedDestination);
        }

        AttachedContainer.EnableMovement();
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
            case PlayerController.Event.ClickHeld: //Both these events do the same thing
                NewState = new PCMovingToLocation();
                NewState.Initialize(AttachedContainer);
                break;
            case PlayerController.Event.DestinationReached:
                NewState = new PCIdle();
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

    public bool IsAtDestination(Vector3 position)
    {
        return Vector3.Distance(position, CachedDestination) <= 0.05f; //Tiny margin of error to account for floating point inaccuracy 
    }
}
