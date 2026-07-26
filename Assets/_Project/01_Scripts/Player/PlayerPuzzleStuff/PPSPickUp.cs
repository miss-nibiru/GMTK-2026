using UnityEngine;

public class PPSPickUp : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private PlayerInteractionInput interactor;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private PlayerPuzzleController puzzleController;
    
    [SerializeField] private PlayerController controller;
    [SerializeField] private FirstPersonCamera fpCam;

    private PickUpItems currentItem;
    
    public void Enter()
    {
        controller.enabled = true;
        fpCam.enabled = true;
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Execute()
    {
        if (interactor.CurrentObject == null || !interactor.CurrentObject.GetComponent<BaseInteractable>()) return;

        if (interactor.CurrentObject.GetComponent<PickUpItems>())
        {
            PickUp();
        }
    }

    public void Exit()
    {
        controller.enabled = false;
        fpCam.enabled = false;
    }

    private void PickUp()
    {
        currentItem = interactor.CurrentObject.GetComponent<PickUpItems>();
        
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
