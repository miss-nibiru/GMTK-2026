using UnityEngine;

public class PlayerPuzzleController : MonoBehaviour
{
    private PlayerPuzzleStateMachine ppsm;
    
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
