using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile Details")]
    [SerializeField] Text playerName;
    [SerializeField] Text playerNumber;
    [SerializeField] Text points;
    [SerializeField] Text gamesWon;
    [SerializeField] Text gamesPlayed;
    [SerializeField] Text playerUploaded;

    [Header("Errors")]
    [SerializeField] GameObject invalidUser;
    [SerializeField] InfoDisplay infoDisplay;

    Dictionary<string, object> playerProfObj;
    Mumble.PlayerProfile playerProfile;

    FirebaseAuth auth;
    FirebaseUser user;
    DatabaseReference dbReference;

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                //Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Verbose;
                InitializeFirebase();
                if(CheckUserProfile() == true)
                {
                    GetPlayerDetails();
                }
            }
            else
            {
                Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus.ToString()));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                infoDisplay.DisplayMessage("Signed out: " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                infoDisplay.DisplayMessage("Signed in: " + user.UserId);
            }
        }
    }

    // Handle removing subscription and reference to the Auth instance.
    // Automatically called by a Monobehaviour after Destroy is called on it.
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

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

    void GetPlayerDetails()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child("+2348141840885")
            .GetValueAsync().ContinueWithOnMainThread(task => {
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
        if(playerProfile == null)
        {
            return; //Exit if there is no valid player data
        }

        playerName.text = playerProfile.number;
        playerNumber.text = playerProfile.number;
        playerUploaded.text = playerProfile.games_uploaded;
        gamesPlayed.text = playerProfile.games_played;
        gamesWon.text = playerProfile.games_won;
        points.text = playerProfile.points;
    }
}
