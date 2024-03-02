using UnityEngine;

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
        LoadScript.LoadScene(1f, "Settings");
    }

    public void GoToMain()
    {
        LoadScript.LoadScene(1f, "Main");
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void GoToProfile()
    {
        LoadScript.LoadScene(1f, "Profile");
    }

    public void GoToPost()
    {
        LoadScript.LoadScene(1f, "Post");
    }
}
