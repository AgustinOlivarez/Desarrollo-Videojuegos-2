using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenuPrincipalCommand : ICommand
{
    public void Execute()
    {
        GameManager.Instance.QuitToMainMenu();
    }
}