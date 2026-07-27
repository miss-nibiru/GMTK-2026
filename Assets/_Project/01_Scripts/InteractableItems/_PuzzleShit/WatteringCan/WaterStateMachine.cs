using UnityEngine;

public class WaterStateMachine
{
    public IWateringCanStates currentState;

    public WSFull fullState;
    public WSEmpty emptyState;
    
    public WaterStateMachine(WateringCanInteractions interact)
    {
        fullState = new WSFull();
        emptyState = new WSEmpty();
    }

    public void SwitchStates(IWateringCanStates state)
    {
        currentState?.Exit();
        currentState = state;
        currentState.Enter();
    }
}
