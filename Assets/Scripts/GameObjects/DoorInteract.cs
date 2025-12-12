using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;
    [SerializeField] bool isBlocked;
    private Animator doorAnimator;
    private bool isOpen = false;


    private void Start()
    {
        if (doorObject != null)
        {
            doorAnimator = doorObject.GetComponent<Animator>();
        }
    }

    public void ToogleDoor()
    {
        if (isBlocked) return;
        isOpen = !isOpen;
        if (isOpen)
        {
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            doorAnimator.SetTrigger("Close");
        }
    }
}
