using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName;
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private string endGameSceneName;

    public void StartGame()
    {
        PlaythroughState.GetOrCreate().BeginNewGame();
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void RestartGame()
    {
        PlaythroughState.GetOrCreate().BeginNewGame();
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OpenEndGame()
    {
        SceneManager.LoadScene(endGameSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}