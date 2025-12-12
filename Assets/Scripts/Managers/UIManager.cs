using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // Variables de los paneles serializados para asignarlos desde el inspector y mantener encapsulacion
    [SerializeField] private GameObject panelPauseMenu;
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject panelInteractGameObject;
    /*[SerializeField] private GameObject panelGameOver;*/ // Todavia no implementado

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
        CameraRaycaster.OnShowInteractPanel += ShowInteract;
        CameraRaycaster.OnHideInteractPanel += HideInteract;
        /* GameManager.OnGameOver += ShowGameOver;*/ // Todavia no implementado
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= ShowHUD;
        GameManager.OnGamePaused -= ShowPauseMenu;
        GameManager.OnGameResumed -= ShowHUD;
        CameraRaycaster.OnShowInteractPanel -= ShowInteract;
        CameraRaycaster.OnHideInteractPanel -= HideInteract;
        /* GameManager.OnGameOver -= ShowGameOver;*/ // Todavia no implementado
    }

    private void ShowHUD()
    {
        panelHUD.SetActive(true);
        panelPauseMenu.SetActive(false);
        panelInteractGameObject.SetActive(false);
        /*panelGameOver.SetActive(false);*/ // Todavia no implementado
    }

    private void ShowPauseMenu()
    {
        panelPauseMenu.SetActive(true);
        panelHUD.SetActive(false);
        panelInteractGameObject.SetActive(false);
        /*panelGameOver.SetActive(false);*/ // Todavia no implementado
    }

    private void ShowGameOver()
    {
        /*panelGameOver.SetActive(true);*/ // Todavia no implementado
        panelHUD.SetActive(false);
        panelPauseMenu.SetActive(false);
        panelInteractGameObject.SetActive(false);
    }

    
    public void ShowInteract()
    {
        panelInteractGameObject.SetActive(true);
    }

    public void HideInteract()
    {
        panelInteractGameObject.SetActive(false);
    }

}
