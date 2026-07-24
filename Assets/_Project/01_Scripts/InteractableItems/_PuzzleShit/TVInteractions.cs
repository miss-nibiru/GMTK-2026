using UnityEngine;
using TMPro;

public class TVInteractions : BaseInteractable
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Transform VHSOutputPos;
    
    [SerializeField] private PlayerPuzzleController puzzleControl;
    private GameObject currentObject;
    private VHSInteractions interactions;
    private bool interactToggle;
    
    public override void Interact()
    {
        switch (interactToggle)
        {
            case false:
                PutVHSIn();
                break;
            case true:
                TakeVHSOut();
                break;
        }
        Debug.Log(interactToggle);
    }

    private void PutVHSIn()
    {
        if (!puzzleControl.currentlyHeldItem) return;

        if (!puzzleControl.currentlyHeldItem.GetComponent<VHSInteractions>()) return;
        
        interactions = puzzleControl.currentlyHeldItem.GetComponent<VHSInteractions>();
        currentObject = puzzleControl.currentlyHeldItem.gameObject;

        text.text = interactions.displayText;
        interactions.pickUpToggle = false;
        currentObject.SetActive(false);
        interactToggle = !interactToggle;
    }

    private void TakeVHSOut()
    {
        text.text = "";
        currentObject.transform.position = VHSOutputPos.position;
        currentObject.SetActive(true);
        currentObject = null;
        interactToggle = !interactToggle;
        Debug.Log("hi");
        PutVHSIn();
    }
}
