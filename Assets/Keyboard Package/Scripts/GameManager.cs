using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private TextMeshProUGUI printBox;

    private const string CorrectAnswer = "SANTIAGO";
    private const string EndSceneName = "03_WinScene";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        textBox.text = "";
        printBox.text = "";
    }

    public void AddLetter(string letter)
    {
        textBox.text += letter;
        printBox.text = "";
    }

    public void DeleteLetter()
    {
        if (textBox.text.Length > 0)
        {
            textBox.text =
                textBox.text.Remove(textBox.text.Length - 1);
        }

        printBox.text = "";
    }

    public void SubmitWord()
    {
        string submittedAnswer = textBox.text.Trim();

        PlaythroughState.GetOrCreate()
            .RecordAnswerAttempt(submittedAnswer);

        if (string.IsNullOrWhiteSpace(submittedAnswer))
        {
            printBox.text = "ENTER A NAME.";
            return;
        }

        if (string.Equals(
                submittedAnswer,
                CorrectAnswer,
                StringComparison.OrdinalIgnoreCase))
        {
            SceneManager.LoadScene(EndSceneName);
            return;
        }

        printBox.text = "THAT DOESN'T FIT THE EVIDENCE.";
        textBox.text = "";
    }
}