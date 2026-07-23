using UnityEngine;

public class PPSPickUp : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private PlayerInteraction interactor;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private PlayerPuzzleController puzzleController;

    private PickUpItems currentItem;
    
    public void Enter()
    {
        interactor.enabled = true;
    }

    public void Execute()
    {
        if (interactor.interactObject == null || !interactor.interactObject.GetComponent<BaseInteractable>()) return;

        if (interactor.interactObject.GetComponent<PickUpItems>())
        {
            PickUp();
        }
    }

    public void Exit()
    {
        interactor.enabled = false;
    }

    private void PickUp()
    {
        currentItem = interactor.interactObject.GetComponent<PickUpItems>();
        
        switch (currentItem.pickUpToggle)
        {
            case true:
                puzzleController.currentlyHeldItem.transform.position = holdPosition.position;
                break;
            case false:
                currentItem = null;
                break;
        }
    }
}
