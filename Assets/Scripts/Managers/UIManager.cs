using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // Variables de los paneles serializados para asignarlos desde el inspector y mantener encapsulacion
    [SerializeField] private GameObject panelPauseMenu;
    [SerializeField] private GameObject panelInfoControls;
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject panelInteractGameObject;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;

    // Variable para los steps a realizar en el juego
    [SerializeField] private TextMeshProUGUI stepText;

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

    private void OnEnable()
    {
        GameManager.OnGameStarted += ShowHUD;
        GameManager.OnGamePaused += ShowPauseMenu;
        GameManager.OnGameResumed += ShowHUD;
        GameManager.OnGameWin += ShowWin;
        GameManager.OnGameLose += ShowLose;
        GameManager.OnInfoControls += ShowInfoControls;
        CameraRaycaster.OnShowInteractPanel += ShowInteract;
        CameraRaycaster.OnHideInteractPanel += HideInteract;
        KeyProgressManager.OnMessageUpdate += UpdateMessage;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= ShowHUD;
        GameManager.OnGamePaused -= ShowPauseMenu;
        GameManager.OnGameResumed -= ShowHUD;
        GameManager.OnGameWin -= ShowWin;
        GameManager.OnGameLose -= ShowLose;
        GameManager.OnInfoControls -= ShowInfoControls;
        CameraRaycaster.OnShowInteractPanel -= ShowInteract;
        CameraRaycaster.OnHideInteractPanel -= HideInteract;
        KeyProgressManager.OnMessageUpdate -= UpdateMessage;
    }

    private void ShowInfoControls()
    {
        panelInfoControls.SetActive(true);
        panelHUD.SetActive(false);
        panelPauseMenu.SetActive(false);
        panelInteractGameObject.SetActive(false);
        panelLose.SetActive(false);
        panelWin.SetActive(false);
    }

    private void ShowHUD()
    {
        panelHUD.SetActive(true);
        panelInfoControls.SetActive(false);
        panelPauseMenu.SetActive(false);
        panelInteractGameObject.SetActive(false);
        panelLose.SetActive(false);
        panelWin.SetActive(false);
    }

    private void ShowPauseMenu()
    {
        panelPauseMenu.SetActive(true);
        panelHUD.SetActive(false);
        panelInteractGameObject.SetActive(false);
        panelLose.SetActive(false);
        panelWin.SetActive(false);
    }

    private void ShowLose()
    {
        panelLose.SetActive(true);
        panelHUD.SetActive(false);
        panelPauseMenu.SetActive(false);
        panelInteractGameObject.SetActive(false);
        panelWin.SetActive(false);
    }

    private void ShowWin()
    {
        panelWin.SetActive(true);
        panelHUD.SetActive(false);
        panelPauseMenu.SetActive(false);
        panelInteractGameObject.SetActive(false);
        panelLose.SetActive(false);
    }

    public void ShowInteract()
    {
        panelInteractGameObject.SetActive(true);
    }

    public void HideInteract()
    {
        panelInteractGameObject.SetActive(false);
    }

    private void UpdateMessage(string message)
    {
        stepText.text = message;
    }

}