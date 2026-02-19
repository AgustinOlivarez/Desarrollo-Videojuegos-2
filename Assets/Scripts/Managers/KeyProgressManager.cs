using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyProgressManager : MonoBehaviour
{
    public static KeyProgressManager Instance { get; private set; }

    [SerializeField] private KeySpawner keySpawner;
    [SerializeField] private List<DoorInteract> doors;

    public static event Action<string> OnMessageUpdate;

    private int currentKeyIndex;
    private KeyInteract currentKey;

    private void Awake()
    {
        // Patron Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SpawnNextKey();
    }

    private void SpawnNextKey()
    {
        if (currentKeyIndex >= doors.Count)
        {
            // Cuando todas las llaves hayan sido recogidas, notificar al final del juego.
            OnMessageUpdate?.Invoke("¡Escapá de la casa para ganar!");
            return;
        }

        OnMessageUpdate?.Invoke($"Ahora encuentra la llave para abrir la puerta: {doors[currentKeyIndex].name}");

        currentKey = keySpawner.Spawn(currentKeyIndex);
        currentKey.OnKeyCollected += OnKeyCollected;
    }

    private void OnKeyCollected(int keyID)
    {
        doors[keyID].Unlock();
        currentKeyIndex++;
        SpawnNextKey();
    }
}