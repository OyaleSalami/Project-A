using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject infoMenu;

    bool sound = true;
    bool mainToggled = true;
    bool infoToggled = false;
    bool settingsToggled = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        SceneManager.LoadScene("Event Scene", LoadSceneMode.Single);
    }

    public void Profile()
    {
        SceneManager.LoadScene("Profile Scene", LoadSceneMode.Single);
    }

    public void Rate()
    {
        Debug.Log("Rate our app on the playstore!");
    }

    public void ChangeTheme()
    {
        Background.UpdateColor();
    }

    public void ToggleSound()
    {
        sound = !sound;
    }

    public void ToggleSetttingsMenu()
    {
        settingsToggled = !settingsToggled;
        if (settingsToggled == true)
        {
            settingsMenu.SetActive(true);
            mainMenu.SetActive(false);
            infoMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(true);
            infoMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
    }

    public void ToggleInfoMenu()
    {
        infoToggled = !infoToggled;
        if (infoToggled == true)
        {
            infoMenu.SetActive(true);
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(true);
            infoMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
    }
}
