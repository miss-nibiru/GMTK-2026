using UnityEngine;

public class PPSPickUp : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private PlayerInteraction interactor;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private PlayerPuzzleController puzzleController;
    
    public PickUpItems currentItem { get; private set; }
    
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
                currentItem.gameObject.transform.position = holdPosition.position;
                break;
            case false:
                currentItem = null;
                break;
        }

        puzzleController.currentlyHeldItem = currentItem.gameObject;
    }
}
