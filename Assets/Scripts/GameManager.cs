using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Header("Sound Control")]
    public bool sound = true;

    [Header("Color Gradient")]
    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] UIGradient gradientObject;

    [Header("UI Control")]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject UtilityMenu;
    [SerializeField] GameObject ProfileMenu;
    [SerializeField] GameObject SettingsMenu;
    private bool settingsToggled = false;
    private bool profileToggled = false;

    public void ChangeColor()
    {
        gradientObject.m_color1 = color1;
        gradientObject.m_color2 = color2;
    }

    public void ToggleSetttingsMenu()
    {
        settingsToggled = !settingsToggled;

        if(settingsToggled == true)
        {
            MainMenu.SetActive(false);
            ProfileMenu.SetActive(false);
            SettingsMenu.SetActive(true);
        }
        else
        {
            MainMenu.SetActive(true);
            ProfileMenu.SetActive(false);
            SettingsMenu.SetActive(false);
        }
    }

    public void ToggleProfileMenu()
    {
        profileToggled =!profileToggled;

        if (profileToggled == true)
        {
            MainMenu.SetActive(false);
            SettingsMenu.SetActive(false);
            ProfileMenu.SetActive(true); //Disable the others and go to the Profile Menu
        }
        else
        {
            MainMenu.SetActive(true);
            SettingsMenu.SetActive(false);
            ProfileMenu.SetActive(false); //Go back to the Main Menu
        }
        Debug.Log("Toggled Profile");
    }

    public void ToggleSound(bool isSound)
    {
        sound = isSound;
    }
}
