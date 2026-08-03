using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerThoughtsUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text thoughtText;
    [SerializeField] private AudioSource voiceSource;
    
    [Header("Repeated Discovery")]
    [SerializeField, TextArea(2, 4)]
    private string repeatedDiscoveryLine;

    private EvidenceTracker _tracker;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float displayDuration = 3.5f;

    private Coroutine _displayRoutine;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        _tracker = EvidenceTracker.GetOrCreate();
        _tracker.DiscoveryReported += HandleDiscoveryReported;
    }

    public void ShowThought(EvidenceData evidence)
    {
        if (evidence == null || string.IsNullOrWhiteSpace(evidence.DetectiveLine))
        {
            return;
        }

        ShowThought(
            evidence.DetectiveLine,
            evidence.DetectiveAudio);
    }

    public void ShowThought(string line, AudioClip voiceClip = null)
    {
        if (string.IsNullOrWhiteSpace(line))
            return;

        if (_displayRoutine != null)
            StopCoroutine(_displayRoutine);

        if (voiceSource != null)
            voiceSource.Stop();

        thoughtText.text = line;
        _displayRoutine = StartCoroutine(
            DisplayRoutine(voiceClip));
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

        ShowThought(repeatedDiscoveryLine);
    }

    private IEnumerator DisplayRoutine(AudioClip voiceClip)
    {
        yield return Fade(0f, 1f);

        if (voiceSource != null && voiceClip != null)
        {
            voiceSource.clip = voiceClip;
            voiceSource.Play();
        }

        float duration = voiceClip != null
            ? Mathf.Max(displayDuration, voiceClip.length)
            : displayDuration;

        yield return new WaitForSecondsRealtime(duration);
        yield return Fade(1f, 0f);

        thoughtText.text = string.Empty;
        _displayRoutine = null;
    }

    private IEnumerator Fade(float start, float end)
    {
        if (fadeDuration <= 0f)
        {
            canvasGroup.alpha = end;
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                start,
                end,
                elapsed / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = end;
    }
}