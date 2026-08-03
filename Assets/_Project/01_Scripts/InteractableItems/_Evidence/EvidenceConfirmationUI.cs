using System;
using System.Collections;
using UnityEngine;

public class EvidenceConfirmationUI : MonoBehaviour
{
    [SerializeField] private EvidenceMenuController menuController;
    [SerializeField] private EvidenceCarouselUI carousel;
    [SerializeField] private PlayerThoughtsUI playerThoughts;
    
    [Header("Timing")]
    [SerializeField, Min(0.1f)]
    private float visibleDuration = 1.25f;

    private Coroutine _confirmationRoutine;
    private EvidenceTracker _tracker;

    private void Awake()
    {
        _tracker = EvidenceTracker.GetOrCreate();
        _tracker.DiscoveryReported += HandleDiscoveryReported;
    }

    private void OnDestroy()
    {
        if (_tracker != null)
            _tracker.DiscoveryReported -= HandleDiscoveryReported;
    }

    private void HandleDiscoveryReported(
        EvidenceDiscoveryReport report)
    {
        if (report.IsFirstDiscovery)
            return;

        PlayConfirmation(
            report.Evidence,
            () =>
            {
                if (playerThoughts != null)
                    playerThoughts.ShowRepeatedDiscoveryThought();
            });
    }
    public void PlayConfirmation(EvidenceData evidence, Action onComplete = null)
    {
        if (evidence == null)
        {
            onComplete?.Invoke();
            return;
        }

        if (_confirmationRoutine != null)
            StopCoroutine(_confirmationRoutine);

        _confirmationRoutine = StartCoroutine(
            ConfirmationRoutine(evidence, onComplete));
    }

    private IEnumerator ConfirmationRoutine(EvidenceData evidence, Action onComplete)
    {
        menuController.OpenEvidence();

        // Wait for EvidenceUI to activate and refresh its slots.
        yield return null;

        bool evidenceWasCentred =
            carousel.CenterOnEvidence(evidence);

        if (!evidenceWasCentred) Debug.LogWarning($"Could not centre evidence '{evidence.EvidenceId}'.", evidence);
        
        yield return null;

        bool confirmationStarted = carousel.PlayCenteredConfirmationShine();

        if (!confirmationStarted) Debug.LogWarning("The centred evidence slot could not play its confirmation.");
        yield return new WaitForSecondsRealtime(visibleDuration);

        menuController.CloseEvidence();
        _confirmationRoutine = null;
        onComplete?.Invoke();
    }
}