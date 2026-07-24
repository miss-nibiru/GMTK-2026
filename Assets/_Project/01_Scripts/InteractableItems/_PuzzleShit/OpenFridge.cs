using UnityEngine;

public class OpenFridge : BaseInteractable
{
    [SerializeField] private GameObject frozenCar;
    
    public override void Interact()
    {
        Instantiate(frozenCar, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
