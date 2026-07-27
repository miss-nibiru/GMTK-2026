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
        if (puzzleControl == null || puzzleControl.CurrentlyHeldItem == null) return;

        if (!puzzleControl.CurrentlyHeldItem.GetComponent<VHSInteractions>()) return;
        
        interactions = puzzleControl.CurrentlyHeldItem.GetComponent<VHSInteractions>();
        currentObject = puzzleControl.CurrentlyHeldItem.gameObject;

        text.text = interactions.displayText;
        interactions.pickUpToggle = false;
        currentObject.SetActive(false);
        interactToggle = !interactToggle;
    }

    private void TakeVHSOut()
    {
        text.text = "";
        currentObject.transform.position = VHSOutputPos.position;
        interactions.pickUpToggle = false;
        currentObject.SetActive(true);
        currentObject = null;
        interactToggle = !interactToggle;
        Debug.Log("hi");
        PutVHSIn();
    }
}
