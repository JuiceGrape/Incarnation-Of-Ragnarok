using UnityEngine;
public class PCAttacking : IPlayerControllerState
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

        if (TargetLeftReach(AttachedContainer.transform.position))
        {
            AttachedContainer.AddEvent(PlayerController.Event.TargetLeftReach);
        }
        else
        {
            AttachedContainer.AttackTarget();
        }
        
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
            case PlayerController.Event.TargetLeftReach:
            case PlayerController.Event.TargetClicked: //Same behaviour
                NewState = new PCMovingToTarget();
                NewState.Initialize(AttachedContainer);
                break;
            case PlayerController.Event.TargetDied:
                NewState = new PCIdle();
                NewState.Initialize(AttachedContainer);
                break;
            default:
                //Unhandled event
                break;
        }

        return NewState;
    }

    bool TargetLeftReach(Vector3 position)
    {
        if (CachedTarget == null)
            return false;

        return CachedTarget.GetDistanceTo(position) > CachedInteractionDistance * 1.2;
    }
}
