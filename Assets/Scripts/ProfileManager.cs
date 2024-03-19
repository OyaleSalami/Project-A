using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using Mumble;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile Details")]
    [SerializeField] private Image displayImage;
    [SerializeField] private Text displayName;
    [SerializeField] private Text postsUploaded;

    [Header("Profile Update")]
    [SerializeField] private Image newImage;
    [SerializeField] private GameObject imageText;
    [SerializeField] private GameObject imagePlaceholder;
    [SerializeField] private InputField newDisplayName;

    /// <summary>Reference to this user's details</summary>
    private User thisUser;

    /// <summary>The bytes of the image to be uploaded</summary>
    private byte[] imageData; //

    private void Start()
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

    private void LoadImage()
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

    private IEnumerator LoadImageFromCloud(string _url)
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

    private void LoadPlayerDetails()
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

    public void Reload()
    {
        LoadScript.LoadScene(1f, "Profile");
    }

    #region Update Profile
    /// <summary>Brings up the context window to allow selecting a file</summary>
    public void SelectFile()
    {
        //Define the extension types
        string pngType = NativeFilePicker.ConvertExtensionToFileType("png");
        string jpgType = NativeFilePicker.ConvertExtensionToFileType("jpg");

        RequestPermissionAsynchronously(); //Request for file permissions on android

        //Select the Image //It calls "CheckImage" when it is done!
        NativeFilePicker.PickFile(CheckImage, pngType, jpgType);
    }

    /// <summary>Checks if the image is valid</summary>
    private void CheckImage(string path)
    {
        //No Image was picked
        if (path == null) return; //Exit the function
        else //A file was picked
        {
            Texture2D texture = new(2, 2); //Create a dummy texture
            imageData = File.ReadAllBytes(path); //Read the file from the path

            //Try converting the bytes to a texture
            if (ImageConversion.LoadImage(texture, imageData) != true)
            {
                imageData = null; return; //Invalid Image //Clear the image data that was loaded
            }
            else if (imageData.Length > 7000000)
            {

                imageData = null; return;//Clear the image data that was loaded //Image is greater than 7mb
            }

            UpdateDisplayImage(); //Valid Image //Load Image Bytes //Safe Size
        }
    }

    public void UpdateDisplayImage()
    {
        imagePlaceholder.SetActive(false); //Disable Placeholders

        Texture2D texture = new(2, 2); //Dummy texture
        ImageConversion.LoadImage(texture, imageData); //Load the image from imageData into a texture

        //Display the selected image
        newImage.color = Color.white;
        newImage.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
                                Vector2.zero);
        newImage.preserveAspect = true;
    }

    public void CheckInput()
    {
        if (newDisplayName.text != "" & imageData != null)
        {
            //Update Profile
            string id = GameManager.instance.user.UserId;
            Firebase.Auth.UserProfile newProfile = new();

            //Create the "image" metadata type for the file to be uploaded
            var newMetaData = new MetadataChange();
            newMetaData.ContentType = "image/jpeg";

            //Get the upload reference for the file
            StorageReference uploadRef = GameManager.instance.stReference.Child("users/" + id + ".jpeg");
            //Upload it asynchronously
            uploadRef.PutBytesAsync(imageData, newMetaData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled) Debug.Log(task.Exception.ToString());
                else
                {
                    LoadScript.LoadScene(1f, "Profile");//Image succesfully uploaded
                }
            }
            );

            newProfile.DisplayName = newDisplayName.text;
            newProfile.PhotoUrl = new (uploadRef.Path);
            GameManager.instance.user.UpdateUserProfileAsync(newProfile);
        }
        else
        {
            return;
        }
    }

    public void UpdateProfile()
    {
        CheckInput();
    }

    /// <summary>Request permission to read a file on mobile</summary>
    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
    }
    #endregion
}
