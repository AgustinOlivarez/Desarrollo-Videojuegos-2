using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private PopupEventSO popupEvent;

    // Popup para volver al menu principal
    public void ShowReturnToMenuPopup()
    {
        ICommand quitCommand = new QuitToMenuPrincipalCommand();
        popupEvent.Raise(
            "Salir",
            "¿Seguro que quieres volver al menú principal?",
            quitCommand
        );
    }
}