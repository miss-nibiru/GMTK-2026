using UnityEngine;

/// <summary>
/// Allows PlayerInteractionInput to discover evidence using E.
/// </summary>
[RequireComponent(typeof(EvidenceDiscoverable))]
public class EvidenceInteractable : BaseInteractable
{
    [SerializeField] private bool hideObjectAfterDiscovery;

    private EvidenceDiscoverable _evidenceDiscoverable;
    

    private void Awake()
    {
        _evidenceDiscoverable =
            GetComponent<EvidenceDiscoverable>();
    }

    public override void Interact()
    {
        if (_evidenceDiscoverable == null)
            return;

        EvidenceData evidence = _evidenceDiscoverable.EvidenceData;

        if (!evidence)
        {
            Debug.LogWarning($"Evidence is not configured on '{gameObject.name}'.", gameObject);
            return;
        }

        if (!evidence.ThoughtOnly && string.IsNullOrWhiteSpace(evidence.EvidenceId))
        {
            Debug.LogWarning($"Evidence is not configured on '{gameObject.name}'.", gameObject);
            return;
        }
        
        _evidenceDiscoverable.ReportDiscovery();

        ApplyDiscoveredState();
    }

    private void ApplyDiscoveredState()
    {
        if (hideObjectAfterDiscovery) gameObject.SetActive(false);
    }
}