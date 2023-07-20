using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Image dailyChallengeImage;

    #region Scene Change
    public void PlayDailyChallenge()
    {
        SceneManager.LoadScene("Daily Event Scene", LoadSceneMode.Single);
    }

    public void PlayGeneralChallenges()
    {
        SceneManager.LoadScene("Event Scene", LoadSceneMode.Single);
    }

    public void LoadProfileScene()
    {
        SceneManager.LoadScene("Profile Scene", LoadSceneMode.Single);
    }

    public void LoadStatsScene()
    {
        Debug.Log("Here are your stats!");
    }

    public void LoadAchievmentsScene()
    {
        Debug.Log("Here are your achievements!");
        //TODO: Call Playgames achievement API
    }

    public void LoadLeaderboardsScene()
    {
        Debug.Log("Here is the leaderboard!");
        //TODO: Call Playgames leaderboard API
    }
    #endregion

    public void CheckDailyEvents()
    {
        //Check cloud and download the images
        //TODO: Check the folder for daily challenges
    }

    public void UpdateDailyEvents()
    {
        //dailyChallengeImage = _;
    }

}
