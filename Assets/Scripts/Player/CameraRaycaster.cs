using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRaycaster : MonoBehaviour
{
    public static event Action OnShowInteractPanel;
    public static event Action OnHideInteractPanel;

    [SerializeField] float distance = 3f;

    [Header("Raycast Layers")]
    [SerializeField] LayerMask interactableMask;
    private bool lookingAtInteractable;

    private PlayerInputAction input;

    private void Awake()
    {
        input = new PlayerInputAction();
    }

    private void Update()
    {
        CheckForPanel();
    }

    private void OnEnable()
    {
        input.Player.InteractObject.performed += OnInteract;

        GameManager.OnGamePaused += DisableRaycaster;
        GameManager.OnGameResumed += EnableRaycaster;
        GameManager.OnGameResumed += ForceCheckInteractPanel;

        EnableRaycaster();
    }

    private void OnDisable()
    {
        input.Player.InteractObject.performed -= OnInteract;

        GameManager.OnGamePaused -= DisableRaycaster;
        GameManager.OnGameResumed -= EnableRaycaster;
        GameManager.OnGameResumed -= ForceCheckInteractPanel;

        input.Player.Disable();
    }

    private void EnableRaycaster()
    {
        input.Player.Enable();
    }

    private void DisableRaycaster()
    {
        input.Player.Disable();
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        TryInteract();
    }

    private void CheckForPanel()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, interactableMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.GetComponentInParent<IInteractable>() is IInteractable interactable)
            {
                if (!lookingAtInteractable)
                {
                    lookingAtInteractable = true;
                    OnShowInteractPanel?.Invoke();
                }
            }
            else
            {
                if (lookingAtInteractable)
                {
                    lookingAtInteractable = false;
                    OnHideInteractPanel?.Invoke();
                }
            }
        }
        else
        {
            if (lookingAtInteractable)
            {
                lookingAtInteractable = false;
                OnHideInteractPanel?.Invoke();
            }
        }

    }

    private void ForceCheckInteractPanel()
    {
        // Recalculo si estaba mirando un objeto interacteable (Puerta, Switch)
        lookingAtInteractable = false;
        CheckForPanel();
    }

    private void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, interactableMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.GetComponentInParent<IInteractable>() is IInteractable interactable)
            {
                interactable.Interact();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }


}