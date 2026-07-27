using UnityEngine;

public class OpenDoor : BaseInteractable
{
    [SerializeField] private Animator anim;
    
    public override void Interact() 
    {
        Debug.Log("hi");
        anim.SetBool("opening", !anim.GetBool("opening"));
    }
}
