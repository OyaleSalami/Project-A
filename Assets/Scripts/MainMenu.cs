using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGeneralChallenges()
    {
        SceneManager.LoadScene("Event Scene", LoadSceneMode.Single);
    }

    public void LoadProfileScene()
    {
        SceneManager.LoadScene("Profile Scene", LoadSceneMode.Single);
    }

    public void LoadUploadScene()
    {
        SceneManager.LoadScene("Upload Scene", LoadSceneMode.Single);
    }
}
