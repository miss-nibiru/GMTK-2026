using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    private void Start()
    {
        EnableCursor();
    }

    private void OnEnable()
    {
        EnableCursor();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            EnableCursor();
        }
    }

    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}