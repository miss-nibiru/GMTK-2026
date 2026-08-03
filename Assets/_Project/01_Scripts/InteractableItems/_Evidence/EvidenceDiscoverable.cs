using UnityEngine;

/// <summary>
/// Connects a physical clue to its EvidenceData.
/// Puzzle scripts may call DiscoverEvidence through a UnityEvent.
/// </summary>
public class EvidenceDiscoverable : MonoBehaviour
{
    [SerializeField] private EvidenceData evidenceData;

    public EvidenceData EvidenceData => evidenceData;

    private void Awake()
    {
        EvidenceTracker
            .GetOrCreate()
            .RegisterEvidence(evidenceData);
    }

    // Keep this void method for UnityEvents and existing puzzle scripts.
    public void DiscoverEvidence()
    {
        ReportDiscovery();
    }

    public bool ReportDiscovery()
    {
        return EvidenceTracker
            .GetOrCreate()
            .DiscoverEvidence(evidenceData);
    }
}