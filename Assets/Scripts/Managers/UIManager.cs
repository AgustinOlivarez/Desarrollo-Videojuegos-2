using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Variables de los paneles serializados para asignarlos desde el inspector y mantener encapsulacion
    [SerializeField] private GameObject panelPauseMenu;
    [SerializeField] private GameObject panelHUD;
    /*[SerializeField] private GameObject panelGameOver;*/ // Todavia no implementado

    private void OnEnable()
    {
        GameManager.OnGameStarted += ShowHUD;
        GameManager.OnGamePaused += ShowPauseMenu;
        GameManager.OnGameResumed += ShowHUD;
        /* GameManager.OnGameOver += ShowGameOver;*/ // Todavia no implementado
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= ShowHUD;
        GameManager.OnGamePaused -= ShowPauseMenu;
        GameManager.OnGameResumed -= ShowHUD;
        /* GameManager.OnGameOver -= ShowGameOver;*/ // Todavia no implementado
    }

    private void ShowHUD()
    {
        panelHUD.SetActive(true);
        panelPauseMenu.SetActive(false);
        /*panelGameOver.SetActive(false);*/ // Todavia no implementado
    }

    private void ShowPauseMenu()
    {
        panelPauseMenu.SetActive(true);
        panelHUD.SetActive(false);
        /*panelGameOver.SetActive(false);*/ // Todavia no implementado
    }

    private void ShowGameOver()
    {
        /*panelGameOver.SetActive(true);*/ // Todavia no implementado
        panelHUD.SetActive(false);
        panelPauseMenu.SetActive(false);
    }

}
