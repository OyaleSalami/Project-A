using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    #region Theme Change
    public static void ChangeTheme(GameManager.Themes theme)
    {
        switch (theme)
        {
            case GameManager.Themes.Light:
                GameManager.textColor = new Color(0, 0, 0);
                GameManager.buttonColor = new Color(0, 0, 0);
                GameManager.backgroundColor = new Color(0, 0, 0);
                break;

            case GameManager.Themes.Dark:
                GameManager.textColor = new Color(0, 0, 0);
                GameManager.buttonColor = new Color(0, 0, 0);
                GameManager.backgroundColor = new Color(0, 0, 0);
                break;

            case GameManager.Themes.Moonlight:
                GameManager.textColor = new Color(0, 0, 0);
                GameManager.buttonColor = new Color(0, 0, 0);
                GameManager.backgroundColor = new Color(0, 0, 0);
                break;

            default:
                //Theme Light
                break;
        }
    }
    #endregion

    public void RateApp()
    {
        Debug.Log("Rate our app on the playstore!");
        //TODO: Call Playstore API for rating apps
    }

    public void AboutApp()
    {

    }

    public void HowTo()
    {

    }

    public void ShareApp()
    {

    }
}
