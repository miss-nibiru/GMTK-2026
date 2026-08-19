using UnityEngine;

public class OpenDoor : BaseInteractable
{
    private static readonly int Opening = Animator.StringToHash("opening");

    [Header("Door Animation")]
    [SerializeField] private Animator anim;

    [Header("Lock")]
    [SerializeField] private bool needsKey;
    [SerializeField] private KeyType requiredKeyType = KeyType.Pantry;
    [SerializeField] private EvidenceData lockedDoor;

    [Header("Player")]
    [SerializeField] private PlayerPuzzleController puzzleControl;

    public override void Interact()
    {
        // Only check held items if this door is actually locked.
        if (needsKey)
        {
            if (puzzleControl == null)
            {
                Debug.LogWarning($"{name}: Door needs a key but PlayerPuzzleController is not assigned.");
                return;
            }

            var heldItem = puzzleControl.CurrentlyHeldItem;

            GetKey heldKey = heldItem != null
                ? heldItem.GetComponent<GetKey>()
                : null;

            // No key OR wrong key.
            if (heldKey == null || heldKey.Type != requiredKeyType)
            {
                if (lockedDoor != null)
                {
                    EvidenceTracker tracker = EvidenceTracker.GetOrCreate();

                    if (tracker != null)
                        tracker.DiscoverEvidence(lockedDoor);
                }

                return;
            }

            // Correct key.
            needsKey = false;
            puzzleControl.ConsumeHeldItem();
        }

        if (anim == null)
        {
            Debug.LogWarning($"{name}: OpenDoor has no Animator assigned.");
            return;
        }

        bool isOpening = !anim.GetBool(Opening);

        anim.SetBool(Opening, isOpening);

        if (isOpening)
            AudioManager.Instance?.PlayOpenDoor();
    }
}