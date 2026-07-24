using UnityEngine;

public class PPSPickUp : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private PlayerInteractionInput interactor;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private PlayerPuzzleController puzzleController;

    private PickUpItems currentItem;
    
    public void Enter()
    {
        interactor.enabled = true;
    }

    public void Execute()
    {
        if (interactor.currentObject == null || !interactor.currentObject.GetComponent<BaseInteractable>()) return;

        if (interactor.currentObject.GetComponent<PickUpItems>())
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
        currentItem = interactor.currentObject.GetComponent<PickUpItems>();
        
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
