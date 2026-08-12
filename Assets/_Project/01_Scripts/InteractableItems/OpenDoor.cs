using UnityEngine;

public class OpenDoor : BaseInteractable
{
    private static readonly int Opening = Animator.StringToHash("opening");
    [SerializeField] private Animator anim;
    [SerializeField] private bool needsKey;
    [SerializeField] private EvidenceData lockedDoor;

    [SerializeField] private PlayerPuzzleController puzzleControl;
    
    public override void Interact()
    {
        var heldItem = puzzleControl.CurrentlyHeldItem;

        if (needsKey)
        {
            GetKey heldKey = null;

            if (heldItem != null) heldKey = heldItem.GetComponent<GetKey>();
            

            if (heldKey == null)
            {
                if (lockedDoor != null)
                {
                    EvidenceTracker tracker = EvidenceTracker.GetOrCreate();
                    if (tracker != null) tracker.DiscoverEvidence(lockedDoor);
                    
                }

                return;
            }

            needsKey = false;
            puzzleControl.ConsumeHeldItem();
        }

        bool isOpening = !anim.GetBool(Opening);
        anim.SetBool(Opening, isOpening);

        if (isOpening)
            AudioManager.Instance?.PlayOpenDoor();
        
    }
}
