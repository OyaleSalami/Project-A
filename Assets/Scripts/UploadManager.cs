using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UploadManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputField titleInput;
    [SerializeField] InputField descInput;
    [SerializeField] Text timerText;

    [Header("Display Image")]
    [SerializeField] Image displayImage;
    [SerializeField] GameObject imageText;
    [SerializeField] GameObject imagePlaceholder;

    [Header("Warning Feedback")]
    [SerializeField] InfoDisplay infoDisplay;

    FirebaseAuth auth;
    FirebaseUser user;
    StorageReference storageReference;
    DatabaseReference databaseReference;

    private byte[] imageData;
    int time = 2; //2 min is the lowest time

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                //Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Verbose;
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus.ToString()));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        UpdateTimeUI();
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        //Initialize Database Reference
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //Initialize Storage Reference
        storageReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com");
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

    public void GoToHome()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Authentication", LoadSceneMode.Single);
    }

    bool CheckUserProfile()
    {
        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log(user.DisplayName + " : " + user.PhoneNumber);
            return true;
        }
        else
        {
            //Goto Login
            infoDisplay.DisplayError("You have to login first", 3f);
            Invoke(nameof(GoToLogin), 5f);
            return false;
        }
    }

    public void SelectFile()
    {
        string pngType = NativeFilePicker.ConvertExtensionToFileType("png");
        string jpgType = NativeFilePicker.ConvertExtensionToFileType("jpg");

        RequestPermissionAsynchronously(); //Request for file permission on android

        //It calls CheckImage when it is done!
        NativeFilePicker.PickFile(CheckImage, pngType, jpgType); //Select the Image
    }

    public void CheckImage(string path)
    {
        if (path == null)
        {
            //Nothing was picked
            //Display Error Message
            infoDisplay.DisplayError("No image was selected");
        }
        else
        {
            Texture2D texture = new Texture2D(2, 2);
            imageData = File.ReadAllBytes(path); //Read the file

            //Check if it is a valid Image
            if (ImageConversion.LoadImage(texture, imageData) != true)
            {
                //Inavlid Image
                imageData = null;
                infoDisplay.DisplayError("The selected image is invalid");
            }
            else
            {
                if (imageData.Length > 7000000) //7mb
                {
                    //TODO: Limit size too
                    imageData = null;
                    infoDisplay.DisplayError("The selected image file is too large");
                }
                else
                {
                    //TODO: Valid Image //Load Image Bytes //Safe Size
                    UpdateDisplayImage();
                }
            }
        }
    }

    public void UploadEvent()
    {
        if (CheckIfValid() == false)
        {
            return; //Exit, Do not upload (The Event is not valid)
        }

        Mumble.Event _eve = new Mumble.Event
        {
            title = titleInput.text,
            description = descInput.text,
            countdown_time = time,
        };
        string output = JsonConvert.SerializeObject(_eve);

        /*Upload Event*/
        string key = databaseReference.Child("events").Push().Key; //Event ID
        _eve.event_id = key;

        databaseReference.Child("events").Child(key).SetRawJsonValueAsync(output).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                //Add event to active list of events (Only if the event uploaded successfully)
                databaseReference.Child("event_list").Push().SetValueAsync(key);
            }
        }
        );

        //Upload Image
        UploadImageToBucket(key);

        //TODO: Upload to user's profile(db)
    }

    void UploadImageToBucket(string id)
    {
        var newMetaData = new MetadataChange();
        newMetaData.ContentType = "image/jpeg";

        StorageReference uploadRef = storageReference.Child("uploads/" + id + ".jpeg");

        uploadRef.PutBytesAsync(imageData, newMetaData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                infoDisplay.DisplayMessage("Image Uploaded Succesfully");
            }
        }
        );
    }

    bool CheckIfValid()
    {
        if (titleInput.text == "" || titleInput.text == null)
        {
            infoDisplay.DisplayError("No title has been set!");
            return false; //It is not valid
        }

        if (imageData == null)
        {
            infoDisplay.DisplayError("Np image has been selected");
            return false; //It is not valid
        }
        return true;//It is valid
    }

    private void UpdateDisplayImage()
    {
        //Disable Placeholders
        imageText.SetActive(false); imagePlaceholder.SetActive(false);

        //TODO: Set display Image
        Texture2D texture = new Texture2D(2, 2);
        ImageConversion.LoadImage(texture, imageData);

        displayImage.color = Color.white;
        displayImage.sprite = Sprite.Create(texture,
                                new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
                                Vector2.zero);
    }

    #region Timer
    public void AddTime()
    {
        time += 1;
        if (time >= 10)
        {
            time = 10; //59 minutes is the highest
        }
        UpdateTimeUI();
    }

    public void ReduceTime()
    {
        time -= 1;
        if (time <= 2)
        {
            time = 2; //5 minutes is the lowest time
        }
        UpdateTimeUI();
    }

    public void UpdateTimeUI()
    {
        timerText.text = time.ToString() + ":00";
    }
    #endregion

    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
    }
}