using UnityEngine;
public class PCDefaultBehaviour : IPlayerControllerState, IDestinationCalculator
{
    private IPlayerControllerState SubState;

    public PCDefaultBehaviour()
    {
        
    }

    public override void Do()
    {
        SubState.Do(); 
    }

    public override void Entry()
    {
        //Empty
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
        switch(pEvent)
        {
            case PlayerController.Event.AbilityActivated:
                NewState = new PCAbilityUsing();
                NewState.Initialize(AttachedContainer);

                break;
            default:
                HandleSubstateEvent(pEvent);
                break;
        }

        return NewState;
    }

    public override void Initialize(PlayerController container)
    {
        base.Initialize(container);
        SubState = new PCIdle(); //Create substate on entry
        SubState.Initialize(container);
        SubState.Entry();
    }

    public bool IsAtDestination(Vector3 position)
    {
        var calculator = SubState as IDestinationCalculator;
        if (calculator != null)
            return calculator.IsAtDestination(position);
        else
            return false;
    }

    private void HandleSubstateEvent(PlayerController.Event pEvent)
    {
        var prevSubState = SubState;

        SubState = SubState.HandleEvent(pEvent);

        if (prevSubState != SubState)
        {
            Debug.Log(SubState);

            prevSubState.Exit();
            SubState.Entry();
        }
    }
}
