using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseButtonUI;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);  // oculta el panel
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
        pauseMenuUI.SetActive(false);   // Oculta el menú
        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(true); // Vuelve a mostrar el botón del HUD
        Time.timeScale = 1f;            // Reanuda el tiempo
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);    // Muestra el menú
        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(false);
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