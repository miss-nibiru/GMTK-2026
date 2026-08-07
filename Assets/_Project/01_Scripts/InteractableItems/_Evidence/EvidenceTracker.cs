using System;
using System.Collections.Generic;
using UnityEngine;

public readonly struct EvidenceDiscoveryReport
{
    public EvidenceDiscoveryReport(
        EvidenceData evidence,
        bool isFirstDiscovery)
    {
        Evidence = evidence;
        IsFirstDiscovery = isFirstDiscovery;
    }

    public EvidenceData Evidence { get; }
    public bool IsFirstDiscovery { get; }
}

/// <summary>
/// Connects EvidenceData assets to the persistent PlaythroughState.
/// PlaythroughState owns the permanent evidence IDs.
/// </summary>

[DefaultExecutionOrder(-9000)]
public class EvidenceTracker : MonoBehaviour
{
    public static EvidenceTracker Instance { get; private set; }

    private readonly List<EvidenceData> _discoveredEvidence = new();

    private readonly Dictionary<string, EvidenceData> _knownEvidence =
        new(StringComparer.Ordinal);

    private PlaythroughState _playthroughState;

    public IReadOnlyList<EvidenceData> DiscoveredEvidence =>
        _discoveredEvidence;

    // Existing UI can continue listening to this
    // It fires only for new evidence
    public event Action<EvidenceData> EvidenceDiscovered;
    public event Action<EvidenceData> ThoughtOnlyReported;
    // It fires for both first and repeated discoveries
    public event Action<EvidenceDiscoveryReport> DiscoveryReported;
    

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        GetOrCreate();
    }

    public static EvidenceTracker GetOrCreate()
    {
        if (Instance != null)
            return Instance;

        GameObject trackerObject = new("[Persistent] EvidenceTracker");
        return trackerObject.AddComponent<EvidenceTracker>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playthroughState = PlaythroughState.GetOrCreate();
        _playthroughState.NewGameStarted += HandleNewGameStarted;

        RebuildDiscoveredEvidence();
    }

    private void OnDestroy()
    {
        if (_playthroughState != null)
            _playthroughState.NewGameStarted -= HandleNewGameStarted;

        if (Instance == this)
            Instance = null;
    }

    public bool DiscoverEvidence(EvidenceData evidence)
    {
        if (!evidence) return false;
        
        if (evidence.ThoughtOnly)
        {
            ThoughtOnlyReported?.Invoke(evidence);
            return false;
        }

        if (string.IsNullOrWhiteSpace(evidence.EvidenceId)) return false;

        RegisterEvidence(evidence);

        bool isFirstDiscovery = _playthroughState.TryDiscoverEvidence(evidence.EvidenceId);

        RebuildDiscoveredEvidence();

        if (isFirstDiscovery) EvidenceDiscovered?.Invoke(evidence);
        DiscoveryReported?.Invoke(new EvidenceDiscoveryReport(evidence, isFirstDiscovery));

        Debug.Log(
            isFirstDiscovery
                ? $"First evidence discovery: {evidence.EvidenceId}"
                : $"Repeated evidence discovery: {evidence.EvidenceId}");

        return isFirstDiscovery;
    }

    public void RegisterEvidence(EvidenceData evidence)
    {
        if (evidence == null ||
            string.IsNullOrWhiteSpace(evidence.EvidenceId))
        {
            return;
        }

        string evidenceId = evidence.EvidenceId.Trim();

        if (_knownEvidence.TryGetValue(
                evidenceId,
                out EvidenceData existing) &&
            existing != null &&
            existing != evidence)
        {
            Debug.LogError($"Duplicate Evidence ID '{evidenceId}' is used by " + $"'{existing.name}' and '{evidence.name}'.", evidence);
            return;
        }

        _knownEvidence[evidenceId] = evidence;
        RebuildDiscoveredEvidence();
    }

    public bool HasDiscovered(string evidenceId)
    {
        return _playthroughState != null && _playthroughState.HasDiscoveredEvidence(evidenceId);
    }

    private void HandleNewGameStarted()
    {
        _discoveredEvidence.Clear();
    }

    private void RebuildDiscoveredEvidence()
    {
        if (_playthroughState == null) return;
        _discoveredEvidence.Clear();

        foreach (string evidenceId in _playthroughState.DiscoveredEvidenceIds)
        {
            if (_knownEvidence.TryGetValue(evidenceId, out EvidenceData evidence) && evidence != null)
            {
                _discoveredEvidence.Add(evidence);
            }
        }
    }
}