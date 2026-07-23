using UnityEngine;

public class PPSPickUp : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private PlayerInteraction interactor;
    [SerializeField] private Transform holdPosition;
    
    private PickUpItems currentItem;
    
    public void Enter()
    {
        interactor.enabled = true;
    }

    public void Execute()
    {
        if (interactor.interactObject == null) return;
        
        currentItem = interactor.interactObject.GetComponent<PickUpItems>();
        
        switch (currentItem.pickUpToggle)
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
