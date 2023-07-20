using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance = new GameManager();
    public enum Themes:int
    {
        Light,
        Dark,
        Moonlight
    }

    [Header("Global Settings")]
    [SerializeField] public static bool isMusic = true;
    [SerializeField] public static bool isSoundEffects = true;
    [SerializeField] public static bool noScreenTimeout = true;
    [SerializeField] public static bool dailyNotifications = true;

    [Header("Theme Settings")]
    [SerializeField] public static Color textColor;
    [SerializeField] public static Color buttonColor;
    [SerializeField] public static Color backgroundColor;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }
}
