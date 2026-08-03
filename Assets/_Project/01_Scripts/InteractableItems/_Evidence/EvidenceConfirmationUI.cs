using System;
using System.Collections;
using UnityEngine;

public class EvidenceConfirmationUI : MonoBehaviour
{
    [SerializeField] private EvidenceMenuController menuController;
    [SerializeField] private EvidenceCarouselUI carousel;
    
    [Header("Timing")]
    [SerializeField, Min(0.1f)]
    private float visibleDuration = 1.25f;

    private Coroutine _confirmationRoutine;

    public void PlayConfirmation(
        EvidenceData evidence,
        Action onComplete = null)
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
        // Wait for the centred slot to finish refreshing.
        yield return null;
        
        yield return new WaitForSecondsRealtime(visibleDuration);

        menuController.CloseEvidence();
        _confirmationRoutine = null;
        onComplete?.Invoke();
    }
}