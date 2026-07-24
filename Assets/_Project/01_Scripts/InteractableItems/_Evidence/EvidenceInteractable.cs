using UnityEngine;

/// <summary>
/// discover evidence using the existing interaction system
/// </summary>
[RequireComponent(typeof(EvidenceDiscoverable))] //need this script to work together
public class EvidenceInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool hideObjectAfterDiscovery;
    private EvidenceDiscoverable _evidenceDiscoverable;

    private void Awake()
    {
        _evidenceDiscoverable = GetComponent<EvidenceDiscoverable>();
    }

    private void Start()
    {
        if (IsAlreadyDiscovered()) 
            ApplyDiscoveredState();
        
    }

    public void Interact()
    {
        _evidenceDiscoverable.DiscoverEvidence();
        if (IsAlreadyDiscovered()) ApplyDiscoveredState();
        
    }

    private bool IsAlreadyDiscovered()
    {
        EvidenceData evidence = _evidenceDiscoverable.EvidenceData;
        return EvidenceTracker.Instance != null && evidence != null && 
               EvidenceTracker.Instance.HasDiscovered(evidence.EvidenceId);
    }

    private void ApplyDiscoveredState()
    {
        if (hideObjectAfterDiscovery)
        {
            gameObject.SetActive(false);
            return;
        }

        Destroy(this);
    }
}