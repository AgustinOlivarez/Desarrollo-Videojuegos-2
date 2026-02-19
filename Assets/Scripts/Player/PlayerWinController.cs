using UnityEngine;

public class PlayerWinController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.WinGame();
        }
    }
}