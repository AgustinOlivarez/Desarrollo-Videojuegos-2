using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Eventos para el patron observer
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;
    public static event Action OnGameRestarted;
    public static event Action OnReturnToMenu;

    // Inicializo variables para controlar el estado del juego
    private bool isPaused = false;
    private bool isGameOver = false;

    private void Awake()
    {

    }

    private void Start()
    {
        Play();
    }

    private void Update()
    {
        // Detectar la tecla Escape para pausar o reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                OnResume();
            else
                OnPause();
        }

        // Forzar el cursor segun el estado actual
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Metodo para iniciar el juego
    public void Play()
    {
        // establezco las variables de estado del juego en false
        isGameOver = false;
        isPaused = false;
        Time.timeScale = 1; // reanudo el tiempo del juego

        // Disparo el delegate OnGameStarted para notificar a los subscriptores
        OnGameStarted?.Invoke();
    }

    // Metodo para pausar el juego
    public void OnPause()
    {
        if (isGameOver) return; // si el juego ya termino no hago nada
        isPaused = true;
        Time.timeScale = 0; // detengo el tiempo del juego

        // Disparo el delegate OnGamePaused para notificar a los subscriptores
        OnGamePaused?.Invoke();
    }

    public void OnResume()
    {
        if (isGameOver) return;
        isPaused = false;
        Time.timeScale = 1; // reanudo el tiempo del juego

        // Disparo el delegate OnGameResumed para notificar a los subscriptores
        OnGameResumed?.Invoke();
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // detengo el tiempo del juego
        // Disparo el delegate OnGameOver para notificar a los subscriptores
        OnGameOver?.Invoke();
    }

    public void Restart()
    {
        // Reincio las variables de estado del juego
        isPaused = false;
        isGameOver = false;
        // Reinicio la escena actual
        Time.timeScale = 1; // reanudo el tiempo del juego
        // Cargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Disparo el delegate OnGameRestarted para notificar a los subscriptores
        OnGameRestarted?.Invoke();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1; // reanudo el tiempo del juego
        // Cargar la escena del menu principal
        SceneManager.LoadScene("MenuPrincipal");
        // Disparo el delegate OnReturnToMenu para notificar a los subscriptores   
        OnReturnToMenu?.Invoke();
    }

}
