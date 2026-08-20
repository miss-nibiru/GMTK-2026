using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private Transform playerCharacter;
    [SerializeField] private float sensitivity;
    [SerializeField] private float minimumLookAngle;
    [SerializeField] private float maximumLookAngle;

    private float _verticalRotation;

    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }

    private void Update()
    {
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        float horizontalLook = lookInput.x * sensitivity;
        float verticalLook = lookInput.y * sensitivity;
        _verticalRotation -= verticalLook; _verticalRotation = Mathf.Clamp(_verticalRotation, minimumLookAngle, maximumLookAngle);

        transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        playerCharacter.Rotate(Vector3.up * horizontalLook);
    }
}