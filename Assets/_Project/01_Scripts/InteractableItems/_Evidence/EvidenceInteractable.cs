using UnityEngine;

/// <summary>
/// Allows PlayerInteractionInput to discover evidence using E.
/// </summary>
[RequireComponent(typeof(EvidenceDiscoverable))]
public class EvidenceInteractable : BaseInteractable
{
    [SerializeField] private bool hideObjectAfterDiscovery;

    private EvidenceDiscoverable _evidenceDiscoverable;
    private bool _interactionEnabled = true;

    private void Awake()
    {
        _evidenceDiscoverable =
            GetComponent<EvidenceDiscoverable>();
    }

    public void SetInteractionEnabled(bool enabled)
    {
        _interactionEnabled = enabled;
    }

    public override bool CanInteract()
    {
        return _interactionEnabled;
    }

    public override void Interact()
    {
        if (!CanInteract())
            return;

        if (_evidenceDiscoverable == null)
            return;

        EvidenceData evidence = _evidenceDiscoverable.EvidenceData;

        if (!evidence)
        {
            Debug.LogWarning(
                $"Evidence is not configured on '{gameObject.name}'.",
                gameObject);

            return;
        }

        if (!evidence.ThoughtOnly &&
            string.IsNullOrWhiteSpace(evidence.EvidenceId))
        {
            Debug.LogWarning(
                $"Evidence is not configured on '{gameObject.name}'.",
                gameObject);

            return;
        }

        _evidenceDiscoverable.ReportDiscovery();

        ApplyDiscoveredState();
    }

    private void ApplyDiscoveredState()
    {
        if (hideObjectAfterDiscovery)
            gameObject.SetActive(false);
    }
}