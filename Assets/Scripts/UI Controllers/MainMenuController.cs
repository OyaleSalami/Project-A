using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject utilityPanel;
    [SerializeField] GameObject settingsPanel;

    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject howtoPanel;

    [SerializeField] GameObject exitPrompt;

    [SerializeField] bool sfx;
    [SerializeField] bool music;
    [SerializeField] bool timeout;
    [SerializeField] bool notifications;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (infoPanel.activeSelf || howtoPanel.activeSelf) //Extra Settings Active
            {
                infoPanel.SetActive(false);
                howtoPanel.SetActive(false);
            }
            else if(settingsPanel.activeSelf == true) //Basic Settings Active
            {
                mainPanel.SetActive(true);
                settingsPanel.SetActive(false);
            }
            else //Main Menu Active
            {
                exitPrompt.SetActive(true);
            }
        }
    }


    #region Navigation
    public void GoToSettings()
    {
        mainPanel.SetActive(false);
        utilityPanel.SetActive(false);
        settingsPanel.SetActive(true);

        infoPanel.SetActive(false);
        howtoPanel.SetActive(false);
    }

    public void GoToMain()
    {
        mainPanel.SetActive(true);
        utilityPanel.SetActive(true);
        settingsPanel.SetActive(false);

        infoPanel.SetActive(false);
        howtoPanel.SetActive(false);
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

    public void RateApp()
    {
        //TODO: Call Playstore API for rating apps
    }

    public void FeedBack()
    {
        //TODO: Open an email link 
    }


    public void ShareApp()
    {
        //TODO: Call Playstore API to share the app link
    }
    #endregion


    #region Settings
    public void SetTheme(int theme)
    {
        //TODO: Do something with the selected theme
        Debug.Log("Changed Theme To: " + theme);
    }

    public void SetMusic(bool value)
    {
        //TODO: Change the music value
        Debug.Log("Music: " + value);
    }

    public void SetSfx(bool value)
    {
        //TODO: Change the sfx value
        Debug.Log("SFX: " + value);
    }

    public void SetNotifications(bool value)
    {
        //TODO: Set the notification value
        Debug.Log("Notifications: " + value);
    }

    public void SetTimeout(bool value)
    {
        //TODO: Set th timeout value
        Debug.Log("Timeout: " + value);
    }
    #endregion

}
