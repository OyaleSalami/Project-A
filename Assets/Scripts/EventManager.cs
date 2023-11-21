using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public void BackToHome()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Authentication", LoadSceneMode.Single);
    }
}
