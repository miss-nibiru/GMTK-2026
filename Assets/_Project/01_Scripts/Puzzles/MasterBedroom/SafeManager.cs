using UnityEngine;

public class SafeManager : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");
    [SerializeField] private SafeNumbers[] numbers;
    [SerializeField] private OpenSafe safe;
    [SerializeField] private Animator animator;

    private int _rightNumbers;
    
    public void CheckNumbers()
    {
        _rightNumbers = 0;
        
        foreach (var num in numbers)
        {
            if (num.targetNum == num.currentNum) _rightNumbers++;
        }
        
        if (_rightNumbers >= numbers.Length)
        {
            animator.SetTrigger(Open);
            safe.Interact();
        }
        
    }
}
