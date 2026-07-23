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
    public int LoopCount { get; private set; }
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
        
        //in the future to grow: the player can control a few hours to go back in time?
        
        LoopCount++;
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
