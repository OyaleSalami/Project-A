using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject infoMenu;
    [SerializeField] GameObject howtoMenu;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            
        }
    }
}
