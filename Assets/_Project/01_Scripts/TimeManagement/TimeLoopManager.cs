using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// when the player touches the hourglass the full day gets rewind
/// when this happens all puzzles reset their timer/reset to start
/// 
/// </summary>

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance { get; private set; }
    public int LoopCount =>
        PlaythroughState.Instance != null
            ? PlaythroughState.Instance.LoopCount
            : 0;
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

    public void RewindFullDay()
    {
        PlaythroughState.GetOrCreate().AdvanceLoop();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    
}
