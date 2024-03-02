using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using Mumble;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile Details")]
    [SerializeField] Image displayImage;
    [SerializeField] Text displayName;
    [SerializeField] Text postsUploaded;

    /// <summary>Reference to this user's details</summary>
    User thisUser;

    void Start()
    {
        if (GameManager.instance.isFirebaseReady == false) //Check if firebase is initialiazed
        {
            if (GameManager.instance.Initialize() != true) //Try to initialize it
            {
                throw new Exception("Unable to initialize the Game Manager");
            }
        }
        LoadPlayerDetails();
    }

    void LoadImage()
    {
        //Get the user ID
        string userId = GameManager.instance.user.UserId;

        //Get image reference
        StorageReference imageReference = GameManager.instance.stReference.Child("users/" + userId + ".jpeg");

        //Get the download link for file
        imageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImageFromCloud(Convert.ToString(task.Result)));
            }
            else
            {
                Debug.LogError("Error downloading image!" + task.Exception);
            }
        }
        );
    }

    IEnumerator LoadImageFromCloud(string _url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(_url);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error Downloading the Image!");
        }
        else
        {
            displayImage.color = Color.white;

            //image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            //TODO: Resize image here
            Texture2D text = ((DownloadHandlerTexture)request.downloadHandler).texture;
            displayImage.sprite = Sprite.Create(text, new Rect(Vector2.zero, new Vector2(text.width, text.height)),
                            Vector2.zero);
            displayImage.preserveAspect = true;
        }
    }

    void LoadPlayerDetails()
    {
        //Get the user ID
        string userId = GameManager.instance.user.UserId;

        //Get the player name
        GameManager.instance.dbReference.Child("users").Child(userId).Child("Profile").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<string, object> dict = (Dictionary<string, object>)snapshot.Value;
                thisUser = new User(dict);
            }
        });
        LoadImage();
    }

    public void UpdateUI()
    {
        //Set the display name of the user
        displayName.text = GameManager.instance.user.DisplayName;
        postsUploaded.text = thisUser.postCount.ToString();
        //displayImage
    }

    public void GoToHome()
    {
        LoadScript.LoadScene(1f, "Main");
    }
}
