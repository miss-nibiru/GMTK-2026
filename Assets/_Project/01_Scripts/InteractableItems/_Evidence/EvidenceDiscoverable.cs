using UnityEngine;

/// <summary>
/// MAAAAAX! ---
/// Attach this to an object in the puzzle that can go into the UI
/// You need to assign the DATA as well
/// Call DiscoverEvidence() when the player successfully
/// collects the evidence
/// </summary>
public class EvidenceDiscoverable : MonoBehaviour
{
    [SerializeField] private EvidenceData evidenceData;

    public EvidenceData EvidenceData => evidenceData;

    public void DiscoverEvidence()
    {
        if (EvidenceTracker.Instance == null) return;
        EvidenceTracker.Instance.DiscoverEvidence(evidenceData);
        
    }
}