using UnityEngine;

public class PCMovingToLocation : IPlayerControllerState
{
    private Vector3? CachedDestination;
    public override void Do()
    {
        if (IsAtDestination(AttachedContainer.transform.position))
        {
            AttachedContainer.AddEvent(PlayerController.Event.DestinationReached);
        }
    }

    public override void Entry()
    {
        if (!CachedDestination.HasValue)
        {
            CachedDestination = AttachedContainer.GetMouseGroundPosition();

            if (CachedDestination != Vector3.negativeInfinity)
            {
                AttachedContainer.SetDestination(CachedDestination.Value);
            }
        }
        
        AttachedContainer.EnableMovement();
    }

    public override void Exit()
    {
        //Empty
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
                NewState = new PCMovingToTarget();
                NewState.Initialize(AttachedContainer);
                break;
            default:
                //Unhandled event
                break;
        }

        return NewState;
    }

    bool IsAtDestination(Vector3 position)
    {
        return Vector3.Distance(position, CachedDestination.Value) <= 0.05f; //Tiny margin of error to account for floating point inaccuracy 
    }
}
