using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    //Declaro variables 
    [SerializeField] private Button btnJugar;
    [SerializeField] private Button btnCreditos;
    [SerializeField] private Button btnSalir;

    void Start()
    {
        // Desbloqueo el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Si las variables no son null le agrega un listener a cada boton para que ejecute una accion
        if (btnJugar !=null && btnCreditos != null && btnSalir != null)
        {
            //btnJugar.onClick.AddListener(() => SceneManager.LoadScene("Gameplay"));
            //btnCreditos.onClick.AddListener(() => SceneManager.LoadScene("Gameplay"));
            btnSalir.onClick.AddListener(() => {
                Application.Quit(); // Funciona en build
                Debug.Log("Saliendo..."); // Para testear en unity
                });
        }
    }

    
    void Update()
    {
        
    }
}
