using UnityEngine;

/// <summary>
/// Updated so theres a switch of items more clearly.  so like if the old thing that is held exists, restore the colliders when its put down
/// If exist, attach to holdposition and here reset the values and disable colliders so it doesnt dance around
/// </summary>
public class PpsPickUp : MonoBehaviour, IPlayerPuzzleStates
{
    [SerializeField] private Transform holdPosition;
    [SerializeField] private PlayerPuzzleController puzzleController;
    [SerializeField] private PlayerController controller;
    [SerializeField] private FirstPersonCamera fpCam;

    private GameObject _handledItem;
    private Transform _originalParent;
    private Collider[] _itemColliders;

    public void Enter()
    {
        controller.enabled = true;
        fpCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Execute()
    {
        GameObject heldItem = puzzleController.CurrentlyHeldItem;

        if (heldItem == _handledItem) return;

        if (_handledItem != null)
            DropItem();
        else
            ClearItemData();

        if (heldItem != null)
            AttachItem(heldItem);
    }

    public void Exit()
    {
        controller.enabled = false;
        fpCam.enabled = false;
    }

    private void AttachItem(GameObject item)
    {
        _handledItem = item;
        _originalParent = item.transform.parent;
        _itemColliders = item.GetComponentsInChildren<Collider>(true);

        SetColliders(false);

        item.transform.SetParent(holdPosition, true);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    private void DropItem()
    {
        _handledItem.transform.SetParent(_originalParent, true);
        SetColliders(true);
        ClearItemData();
    }

    private void SetColliders(bool enabled)
    {
        foreach (Collider itemCollider in _itemColliders)
            itemCollider.enabled = enabled;
    }

    private void ClearItemData()
    {
        _handledItem = null;
        _originalParent = null;
        _itemColliders = null;
    }
}