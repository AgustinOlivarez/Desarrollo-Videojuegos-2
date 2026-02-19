using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject doorObject;
    [SerializeField] private bool isBlocked;
    [SerializeField] private NavMeshObstacle doorObstacle;
    private Animator doorAnimator;
    private bool isOpen = false;

    private void Start()
    {
        if (doorObject != null)
        {
            doorAnimator = doorObject.GetComponent<Animator>();
        }
    }

    public void Interact()
    {
        if (isBlocked) return;
        ToogleDoor();
    }

    public void ToogleDoor()
    {
        isOpen = !isOpen;  // Cambia el estado de la puerta
        doorAnimator.SetBool("isOpen", isOpen);  // Actualiza el parámetro en el Animator
    }

    public void Unlock()
    {
        isBlocked = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyOpenDoor();
            StartCoroutine(ResetInteraction());
        }
    }

    private void EnemyOpenDoor()
    {
        if (!isOpen)
        {
            ToogleDoor();
        }
    }

    private IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(1f);
    }
}