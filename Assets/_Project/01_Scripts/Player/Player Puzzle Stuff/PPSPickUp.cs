using UnityEngine;

public class PPSPickUp : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private PlayerInteraction interactor;
    [SerializeField] private Transform holdPosition;
    
    public void Enter()
    {
        interactor.enabled = true;
    }

    public void Execute()
    {
        switch (interactor.pickUpToggle)
        {
            case true:
                interactor.interactObject.transform.position = holdPosition.position;
                break;
            case false:
                
                break;
        }
    }

    public void Exit()
    {
        interactor.enabled = false;
    }
}
