using System.Collections;
using TMPro;
using UnityEngine;

public class LoopReactionUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup thoughtsCanvasGroup;
    [SerializeField] private TMP_Text reactionText;
    [SerializeField] private float displayDuration = 2.5f;

    [Header("Messages")]
    [SerializeField] private string firstRewindMessage = "What just happened?";
    [SerializeField] private string laterRewindMessage = "Here we go again...";

    private IEnumerator Start()
    {
        int loopCount = TimeLoopManager.Instance != null
            ? TimeLoopManager.Instance.LoopCount
            : 0;

        if (loopCount <= 0)
            yield break;

        reactionText.text = loopCount == 1
            ? firstRewindMessage
            : laterRewindMessage;

        thoughtsCanvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(displayDuration);

        thoughtsCanvasGroup.alpha = 0f;
    }
}