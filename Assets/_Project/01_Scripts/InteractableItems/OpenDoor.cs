using UnityEngine;

public class OpenDoor : BaseInteractable
{
    private static readonly int Opening = Animator.StringToHash("opening");
    [SerializeField] private Animator anim;
    [SerializeField] private bool needsKey;

    [SerializeField] private PlayerPuzzleController puzzleControl;
    
    public override void Interact() 
    {
        var heldItem = puzzleControl.CurrentlyHeldItem;

        if (needsKey)
        {
            
            if (!heldItem) return;
            var heldKey = heldItem.GetComponent<GetKey>();
            if (!heldKey) return;
            needsKey = false;
            puzzleControl.ConsumeHeldItem();
            
        }
        
        anim.SetBool(Opening, !anim.GetBool(Opening));
        
    }
}
