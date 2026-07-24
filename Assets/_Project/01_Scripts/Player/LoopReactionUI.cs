using System.Collections;
using TMPro;
using UnityEngine;
/// <summary>
/// everytime the time loops, the player needs to feel that is normal but disoriented at first
/// All subsequent times the character needs to react as it was normal or expected
/// </summary>
public class LoopReactionUI : MonoBehaviour
{
    [SerializeField] private GameObject reactionThought;
    [SerializeField] private TMP_Text reactionText;
    [SerializeField] private float displayDuration;

    [Header("Messages")]
    [SerializeField] private string firstRewindMessage;
    [SerializeField] private string laterRewindMessage;

    private IEnumerator Start()
    {
        
        reactionThought.SetActive(false);

        if (TimeLoopManager.Instance == null || TimeLoopManager.Instance.LoopCount <= 0)
            yield break;

        reactionText.text = TimeLoopManager.Instance.LoopCount == 1 ? firstRewindMessage : laterRewindMessage;
        reactionThought.SetActive(true);
        yield return new WaitForSecondsRealtime(displayDuration);
        reactionThought.SetActive(false);
        
    }
}