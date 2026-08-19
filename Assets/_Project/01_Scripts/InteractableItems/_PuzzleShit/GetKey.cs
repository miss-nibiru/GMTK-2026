using UnityEngine;

public enum KeyType
{
    Pantry,
    FrontDoor
}

public class GetKey : PickUpItems
{
    [SerializeField] private KeyType keyType = KeyType.Pantry;

    public KeyType Type => keyType;
}