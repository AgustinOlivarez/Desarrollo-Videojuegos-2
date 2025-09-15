using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{

    void Start()
    {
        Invoke("WaitToEnd", 16);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}