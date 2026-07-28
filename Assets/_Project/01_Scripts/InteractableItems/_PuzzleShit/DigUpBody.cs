using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DigUpBody : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;
    private Vector3 positionIncrease = new Vector3(0, 0.1f, 0);
    [SerializeField] private GameObject body;
    
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
            StartCoroutine(digTimer());
            return;
        }
        
        StartCoroutine(endPause());
    }

    private IEnumerator digTimer()
    {
        yield return new WaitForSeconds(0.1f);
        body.transform.position += positionIncrease;
        Digging();
    }

    private IEnumerator endPause()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
