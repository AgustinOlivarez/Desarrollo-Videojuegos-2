using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    //Variables
    private Image logo;
    private bool loadFinish;
    private bool endLogo;

    // Metodo awake para inicializar variables antes del primer frame
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloqueo el cursor
        logo = GetComponent<Image>();// Obtengo el logo
        loadFinish = false;
        endLogo = false;
        
    }

    void Start()
    {
        // Aca van a inicializarse los managers

        loadFinish = true; // Cuando cargan todos los managers lo pone en true
        
    }

    void Update()
    {
        if(loadFinish && endLogo)
        {
            SceneManager.LoadScene("MenuPrincipal"); // Carga la escena de menu principal
        }
    }

    //Metodo publico para llamar desde la animacion por un evento
    public void EndAnimationLogo() { endLogo = true; }
}
