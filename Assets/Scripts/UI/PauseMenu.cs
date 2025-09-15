using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

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
        pauseMenuUI.SetActive(false);   // Oculta el men�
        Time.timeScale = 1f;            // Reanuda el tiempo
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);    // Muestra el men�
        Time.timeScale = 0f;            // Detiene el tiempo del juego
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;            // Resetea el tiempo
        SceneManager.LoadScene("MenuPrincipal"); // Asegurate de tener tu escena de men� en Build Settings
    }

}