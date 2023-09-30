using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    #region Settings Property
    public static void ToggleSoundEffects()
    {
        GameManager.isSoundEffects = !GameManager.isSoundEffects;
    }

    public static void ToggleMusic()
    {
        GameManager.isMusic = !GameManager.isMusic;
    }

    public static void ToggleScreenTimeout()
    {
        GameManager.noScreenTimeout = !GameManager.noScreenTimeout;
    }

    public static void ToggleDailyNotifications()
    {
        GameManager.dailyNotifications = !GameManager.dailyNotifications;
    }
    #endregion

    public void RateApp()
    {
        InfoDisplay infoDisplay = FindObjectOfType<InfoDisplay>();
        infoDisplay.DisplayMessage("Rate our app on the playstore!");

        //TODO: Call Playstore API for rating apps
    }

    public void ShareApp()
    {
        InfoDisplay infoDisplay = FindObjectOfType<InfoDisplay>();
        infoDisplay.DisplayMessage("Share our app with others!");

        //TODO: Call Playstore API for sharing the app link
    }
}
