using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject panelPauseMenu;
    public GameObject panelHUD;
    private bool isPaused = false;

    void Start()
    {
        panelPauseMenu.SetActive(false);  // oculta el panel
        Time.timeScale = 1f;       
        isPaused = false;         
    }

    void Update()
    {
        // Detectar tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        EventSystem.current.SetSelectedGameObject(null);
        panelPauseMenu.SetActive(false);   // Oculta el menú
        if (panelHUD != null)
            panelHUD.SetActive(true); // Vuelve a mostrar el botón del HUD
        Time.timeScale = 1f;            // Reanuda el tiempo
        isPaused = false;
    }

    private void Pause()
    {
        panelPauseMenu.SetActive(true);    // Muestra el menú
        if (panelHUD != null)
            panelHUD.SetActive(false);
        Time.timeScale = 0f;            // Detiene el tiempo del juego
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;            // Resetea el tiempo
        SceneManager.LoadScene("MenuPrincipal"); // Asegurate de tener tu escena de menú en Build Settings
    }
    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }
}