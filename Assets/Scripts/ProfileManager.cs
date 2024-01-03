using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile Details")]
    [SerializeField] Text points;
    [SerializeField] Text displayName;
    [SerializeField] Text postsUploaded;

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
                }
            });
    }

    public void UpdateUI()
    {
        //Set the display name of the user
        displayName.text = GameManager.instance.user.DisplayName;
    }
    public void Exit()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
