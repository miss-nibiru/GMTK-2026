using UnityEngine;

public class SafeManager : MonoBehaviour
{
    [SerializeField] private SafeNumbers[] numbers;

    private int rightNumbers;
    
    public void CheckNumbers()
    {
        rightNumbers = 0;
        
        foreach (var num in numbers)
        {
            if (num.targetNum == num.currentNum)
            {
                rightNumbers++;
                continue;
            }
        }

        Debug.Log(rightNumbers);
        
        if (rightNumbers >= numbers.Length)
        {
            Debug.Log("solved puzzle");
        }
    }
}
