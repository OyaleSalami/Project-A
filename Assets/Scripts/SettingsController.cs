using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] Image soundImage;
    [SerializeField] Sprite enabledSound;
    [SerializeField] Sprite disabledSound;

    [Header("Notifications Settings")]
    [SerializeField] Image notificationImage;
    [SerializeField] Sprite enabledNoti;
    [SerializeField] Sprite disabledNoti;

    [Header("Details Panel")]
    [SerializeField] GameObject infoPanel;

    void Start()
    {
        LoadSettings();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            infoPanel.SetActive(false);
        }
    }

    public void SetSettingsValue(string value)
    {
        if (PlayerPrefs.GetInt(value) == 0)
        {
            PlayerPrefs.SetInt(value, 1);
        }
        else
        {
            PlayerPrefs.SetInt(value, 0);
        }

        UpdateUI();
    }

    /// <summary>Checks to see if settings exist, else create them</summary>
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("sound") != true)
        {
            PlayerPrefs.SetInt("sound", 1);
        }

        if (PlayerPrefs.HasKey("notifications") != true)
        {
            PlayerPrefs.SetInt("notifications", 1);
        }

        if (PlayerPrefs.HasKey("theme") != true)
        {
            PlayerPrefs.SetInt("theme", 1); //Light(0) Dark(1)
        }
    }

    void UpdateUI()
    {
        soundImage.sprite = PlayerPrefs.GetInt("sound") == 1 ? enabledSound : disabledSound;
        notificationImage.sprite = PlayerPrefs.GetInt("notifications") == 1 ? enabledNoti : disabledNoti;
    }

    public void RateApp()
    {
        //TODO: Call Playstore rating API
    }

    public void GoToHome()
    {
        LoadScript.LoadScene(1f, "Main");
    }
}
