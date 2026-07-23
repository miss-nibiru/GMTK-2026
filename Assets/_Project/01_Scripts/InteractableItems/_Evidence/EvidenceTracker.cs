using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Remembers evidence discovered during the current play session
/// has to rememebr certain things when the time gets reset
/// </summary>
public class EvidenceTracker : MonoBehaviour
{
    public static EvidenceTracker Instance { get; private set; }
    private readonly List<EvidenceData> _discoveredEvidence = new();
    private readonly HashSet<string> _discoveredIds = new(); //connected to the item id directly, not name

    public IReadOnlyList<EvidenceData> DiscoveredEvidence => _discoveredEvidence;
    public event Action<EvidenceData> EvidenceDiscovered; //add to UI needs to be implemented

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool DiscoverEvidence(EvidenceData evidence)
    {
        if (evidence == null)
        {
            Debug.LogWarning("Cannot discover missing EvidenceData.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(evidence.EvidenceId))
        {
            Debug.LogWarning(
                $"Evidence '{evidence.name}' does not have an Evidence ID.");
            return false;
        }

        if (!_discoveredIds.Add(evidence.EvidenceId)) return false;
        _discoveredEvidence.Add(evidence);
        EvidenceDiscovered?.Invoke(evidence);
        return true;
        
    }

    public bool HasDiscovered(string evidenceId)
    {
        return !string.IsNullOrWhiteSpace(evidenceId) && _discoveredIds.Contains(evidenceId);
    }
}