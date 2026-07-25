using UnityEngine;

public class PlayerPuzzleController : MonoBehaviour
{
    public GameObject currentlyHeldItem;
    public GameObject previouslyHeldItem;
    
    public PlayerPuzzleStateMachine ppsm { get; private set; }
    
    void Start()
    {
        ppsm = new PlayerPuzzleStateMachine(this);
        ppsm.SwitchStates(ppsm.pickUpState);
    }

    // Update is called once per frame
    void Update()
    {
        ppsm.currentState.Execute();
    }
}
