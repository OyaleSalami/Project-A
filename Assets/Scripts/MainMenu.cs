using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadProfile()
    {
        SceneManager.LoadScene("Profile Scene", LoadSceneMode.Single);
    }

    public void PostEvent()
    {
        SceneManager.LoadScene("Upload Scene", LoadSceneMode.Single);
    }
}
