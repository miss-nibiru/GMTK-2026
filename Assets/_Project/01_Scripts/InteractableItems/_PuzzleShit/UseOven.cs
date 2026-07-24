using System.Collections;
using UnityEngine;

public class UseOven : BaseInteractable
{
    [SerializeField] private PlayerPuzzleController puzzleControl;
    
    [SerializeField] private float timer;
    [SerializeField] private GameObject car;
    private bool cookedCar;
    
    public override void Interact()
    {
        if (!puzzleControl.currentlyHeldItem && !cookedCar) return;
        
        switch (cookedCar)
        {
            case false:
                StartCoroutine(cookTimer());
                Destroy(puzzleControl.currentlyHeldItem);
                break;
            case true:
                Instantiate(car, transform.position, transform.rotation);
                Destroy(gameObject);
                break;
        }
    }

    private IEnumerator cookTimer()
    {
        yield return new WaitForSeconds(timer);
        Debug.Log("Ready");
        cookedCar = true;
    }
}
