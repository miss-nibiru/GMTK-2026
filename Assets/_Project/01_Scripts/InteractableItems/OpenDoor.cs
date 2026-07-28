using UnityEngine;

public class OpenDoor : BaseInteractable
{
    [SerializeField] private Animator anim;
    [SerializeField] private bool needsKey;

    [SerializeField] private PlayerPuzzleController puzzleControl;
    
    public override void Interact() 
    {
        if (needsKey)
        {
            if (!puzzleControl.CurrentlyHeldItem.GetComponent<GetKey>()) return;
        }
        
        anim.SetBool("opening", !anim.GetBool("opening"));
        needsKey = false;

        if (puzzleControl.CurrentlyHeldItem.GetComponent<GetKey>())
        {
            puzzleControl.ConsumeHeldItem();
        }
    }
}
