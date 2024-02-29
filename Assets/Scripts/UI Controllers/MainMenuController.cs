using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            ExitApp();
        }
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void GoToProfile()
    {
        SceneManager.LoadScene("Profile", LoadSceneMode.Single);
    }

    public void GoToPost()
    {
        SceneManager.LoadScene("Post", LoadSceneMode.Single);
    }
}
