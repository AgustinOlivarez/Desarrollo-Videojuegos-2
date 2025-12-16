using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public KeyInteract Spawn(int keyID)
    {
        GameObject obj = Instantiate(keyPrefab, spawnPoints[keyID].position, keyPrefab.transform.rotation);
        KeyInteract key = obj.GetComponent<KeyInteract>();
        key.Initialize(keyID);
        return key;
    }
}