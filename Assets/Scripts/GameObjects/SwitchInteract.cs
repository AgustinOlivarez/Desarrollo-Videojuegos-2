using UnityEngine;

public class SwitchInteract : MonoBehaviour
{
    //
    [Header("Lamp Settings")]
    [SerializeField] private GameObject[] lamps;
    private bool isOn = false;

    //
    public void ToggleLamp()
    {
        isOn = !isOn;
        foreach (var l in lamps)
        {
            l.SetActive(isOn);
        }
    }
}
