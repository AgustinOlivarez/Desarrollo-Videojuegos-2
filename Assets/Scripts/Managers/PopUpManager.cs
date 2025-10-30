using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Windows.Input;

public class PopupManager : MonoBehaviour
{
    // Referencia al prefab del popup
    [SerializeField] private GameObject popupPrefab;

    // Lista de eventos de popup
    [SerializeField] private PopupEventSO[] popupEvents;

    //Popup actual
    private GameObject popup;

    private void Update()
    {
        // Si presionan Escape, cierras el último popup
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (popup)
            {
                Destroy(popup);
            }

        }
    }

    private void OnEnable()
    {
        foreach (var p in popupEvents)
        {
            p.OnShowPopup += ShowPopup;
        }
    }

    private void OnDisable()
    {
        foreach (var p in popupEvents)
        {
            p.OnShowPopup -= ShowPopup;
        }
    }

    private void ShowPopup(string title, string message, ICommand buttonCommand)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("PopupManager: No se encontro el Canvas");
            return;
        }

        popup = Instantiate(popupPrefab, canvas.transform);

        TMP_Text titleText = popup.transform.Find("Title").GetComponent<TMP_Text>();
        TMP_Text messageText = popup.transform.Find("Message").GetComponent<TMP_Text>();
        Button acceptButton = popup.transform.Find("Buttons/AcceptButton").GetComponent<Button>();
        Button cancelButton = popup.transform.Find("Buttons/CancelButton").GetComponent<Button>();

        titleText.text = title;
        messageText.text = message;

        acceptButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        acceptButton.onClick.AddListener(() =>
        {
            buttonCommand?.Execute();
            Destroy(popup);
        });

        cancelButton.onClick.AddListener(() =>
        {
            Destroy(popup);
        });
    }

}