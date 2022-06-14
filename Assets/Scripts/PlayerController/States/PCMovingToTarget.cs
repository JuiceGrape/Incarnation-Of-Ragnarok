using UnityEngine;

public class PCMovingToTarget : IPlayerControllerState
{
    Targetable CachedTarget;
    float CachedInteractionDistance = 0.0f;
    public override void Do()
    {
        if (CachedTarget != AttachedContainer.GetTarget())
        {
            CachedTarget = AttachedContainer.GetTarget();

            if (CachedTarget is Hittable)
            {
                CachedInteractionDistance = AttachedContainer.GetAttachedPlayer().GetAttackRange();
            }
            else if (CachedTarget is Interactable)
            {
                CachedInteractionDistance = AttachedContainer.GetInteractionDistance();
            }
        }

        if (IsAtDestination(AttachedContainer.transform.position))
        {
            AttachedContainer.AddEvent(PlayerController.Event.DestinationReached);
        }
        else
        {
            AttachedContainer.SetDestination(CachedTarget.transform.position);
        }
    }

    public override void Entry()
    {
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
                NewState = new PCMovingToLocation();
                NewState.Initialize(AttachedContainer);
                break;
            case PlayerController.Event.DestinationReached:
                if (CachedTarget is Hittable)
                {
                    NewState = new PCAttacking(); //DEBUG CODE, SHOULD BE ATTACK
                    NewState.Initialize(AttachedContainer);
                }
                else if (CachedTarget is Interactable)
                {
                    NewState = new PCIdle();
                    ((Interactable)CachedTarget).Interact(); //Technically, the exit of this state needs to happen before this happens
                }
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
        if (CachedTarget == null)
            return false;

        return CachedTarget.GetDistanceTo(position) <= CachedInteractionDistance;
    }
}