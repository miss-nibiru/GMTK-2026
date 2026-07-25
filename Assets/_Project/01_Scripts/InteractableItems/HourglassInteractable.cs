using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class HourglassInteractable : MonoBehaviour, IInteractable
{
    private const string TimePauseReason = "HourglassRewind";

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip rewindClip;
    [SerializeField] private string rewindTrigger = "Rewind";

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rewindSound;

    private bool _isRewinding;

    private void Reset()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (_isRewinding) return;
        if (TimeLoopManager.Instance == null)
        {
            Debug.LogError(
                "Hourglass cannot rewind because TimeLoopManager is missing.",
                this
            );

            return;
        }

        StartCoroutine(RewindRoutine());
    }

    private IEnumerator RewindRoutine()
    {
        _isRewinding = true;
        GameTimeManager.Instance?.PauseTime(TimePauseReason);

        if (audioSource != null && rewindSound != null)
        {
            audioSource.PlayOneShot(rewindSound);
        }

        if (animator != null && !string.IsNullOrWhiteSpace(rewindTrigger))
        {
            animator.ResetTrigger(rewindTrigger);
            animator.SetTrigger(rewindTrigger);
        }

        if (rewindClip != null) yield return new WaitForSecondsRealtime(rewindClip.length);
        else yield return null;
        TimeLoopManager.Instance.RewindFullDay();
    }
}