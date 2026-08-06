using UnityEngine;

public class SafeManager : MonoBehaviour
{
    [SerializeField] private SafeNumbers[] numbers;

    [SerializeField] private OpenSafe safe;

    private int rightNumbers;
    
    public void CheckNumbers()
    {
        rightNumbers = 0;
        
        foreach (var num in numbers)
        {
            if (num.targetNum == num.currentNum)
            {
                rightNumbers++;
            }
        }

        Debug.Log(rightNumbers);
        
        if (rightNumbers >= numbers.Length)
        {
            safe.Interact();
        }
    }
}
