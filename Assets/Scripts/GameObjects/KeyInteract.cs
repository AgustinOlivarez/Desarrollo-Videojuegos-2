using System;
using UnityEngine;

public class KeyInteract : MonoBehaviour, IInteractable
{
    public event Action<int> OnKeyCollected;

    public int KeyID { get; private set; }

    public void Initialize(int keyID)
    {
        KeyID = keyID;
    }

    public void Interact()
    {
        OnKeyCollected?.Invoke(KeyID);
        Destroy(gameObject);
    }
}