using UnityEngine;

/// <summary>
/// Allows PlayerInteractionInput to discover evidence using E.
/// </summary>
[RequireComponent(typeof(EvidenceDiscoverable))]
public class EvidenceInteractable : BaseInteractable
{
    [SerializeField] private bool hideObjectAfterDiscovery;

    private EvidenceDiscoverable _evidenceDiscoverable;
    private bool _reportedThisLoop;

    private void Awake()
    {
        _evidenceDiscoverable =
            GetComponent<EvidenceDiscoverable>();
    }

    public override void Interact()
    {
        if (_reportedThisLoop || _evidenceDiscoverable == null)
            return;

        EvidenceData evidence = _evidenceDiscoverable.EvidenceData;

        if (evidence == null ||
            string.IsNullOrWhiteSpace(evidence.EvidenceId))
        {
            Debug.LogWarning($"Evidence is not configured on '{gameObject.name}'.", gameObject);
            return;
        }

        _reportedThisLoop = true;
        _evidenceDiscoverable.ReportDiscovery();

        ApplyDiscoveredState();
    }

    private void ApplyDiscoveredState()
    {
        if (hideObjectAfterDiscovery)
        {
            gameObject.SetActive(false);
            return;
        }

        // Prevent another interaction during this loop.
        // Reloading restores the physical object for the next loop.
        Destroy(this);
    }
}