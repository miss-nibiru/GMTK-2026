using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DigUpBody : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;
    private readonly Vector3 _positionIncrease = new Vector3(0, 0.1f, 0);
    [SerializeField] private GameObject body;

    public override bool CanInteract()
    {
        if (!puzzleControl.CurrentlyHeldItem) return false;
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<UseShovel>()) return false;
        return true;
    }

    public override void Interact()
    {
        if (!puzzleControl.CurrentlyHeldItem) return;
        
        if (!puzzleControl.CurrentlyHeldItem.GetComponent<UseShovel>()) return;
        
        Digging();
    }

    private void Digging()
    {
        if (body.transform.position.y < 0)
        {
            StartCoroutine(DigTimer());
            return;
        }
        
        StartCoroutine(EndPause());
    }

    private IEnumerator DigTimer()
    {
        yield return new WaitForSeconds(0.1f);
        body.transform.position += _positionIncrease;
        Digging();
    }

    private static IEnumerator EndPause()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}