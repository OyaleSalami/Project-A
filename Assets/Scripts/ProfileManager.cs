using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile Details")]
    [SerializeField] Text playerName;
    [SerializeField] Text playerNumber;
    [SerializeField] Text points;
    [SerializeField] Text gamesWon;
    [SerializeField] Text gamesPlayed;
    [SerializeField] Text playerUploaded;

    Dictionary<string, object> playerProfObj;
    Mumble.PlayerProfile playerProfile;

    void Start()
    {
        if (GameManager.instance.isFirebaseReady == true)
        {
            //CheckUserProfile();
            GetPlayerDetails();
        }
        else
        {
            //TODO: Throw an error
        }
    }

    /* Check the player profile
    bool CheckUserProfile()
    {
        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log(user.DisplayName + " : " + user.PhoneNumber);
            //TODO: Load profile details from here

            return true;
        }
        else
        {
            Debug.Log("Invalid User!");
            //Goto Login
            infoDisplay.DisplayError("You have to sign in first", 3f);
            Invoke(nameof(GoToLogin), 5f);

            return false;
        }
    }
    */

    void GetPlayerDetails()
    {
        GameManager.instance.dbReference.Child("users").Child("+2348141840885")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    // Do something with snapshot...
                    playerProfObj = new Dictionary<string, object>();
                    playerProfObj = (Dictionary<string, object>)snapshot.Value;

                    playerProfile = new Mumble.PlayerProfile(playerProfObj);
                    UpdateUI();
                }
            });
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Authentication", LoadSceneMode.Single);
    }

    public void UpdateUI()
    {
        if (playerProfile == null)
        {
            return; //Exit if there is no valid player data
        }
    }
}
