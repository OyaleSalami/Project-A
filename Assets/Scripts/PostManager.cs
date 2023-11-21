using Firebase.Extensions;
using Firebase.Storage;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputField descInput;
    [SerializeField] InputField titleInput;

    [Header("Display Image")]
    [SerializeField] Image displayImage;
    [SerializeField] GameObject imageText;
    [SerializeField] GameObject imagePlaceholder;

    [Header("Payments")]
    [SerializeField] PayManager payManager;

    private byte[] imageData;
    int time = 2; //2 min is the lowest time

    void Start()
    {
        if (GameManager.instance.isFirebaseReady)
        {

        }
        else
        {
            //Throw error
        }
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Auth", LoadSceneMode.Single);
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
        if (path == null) //No Image was picked
        {
            GameManager.instance.infoDisplay.DisplayError("No image was selected");
        }
        else
        {
            Texture2D texture = new Texture2D(2, 2);
            imageData = File.ReadAllBytes(path); //Read the file from the path

            if (ImageConversion.LoadImage(texture, imageData) != true) //Invalid Image
            {
                imageData = null;
                GameManager.instance.infoDisplay.DisplayError("The selected image is invalid");
            }
            else //Valid Image
            {
                if (imageData.Length > 7000000) //Image is greater than 7mb
                {
                    imageData = null;
                    GameManager.instance.infoDisplay.DisplayError("The selected image file is too large");
                }
                else 
                {
                    //Valid Image //Load Image Bytes //Safe Size
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

        /*Upload Event*/
        string key = GameManager.instance.dbReference.Child("events").Push().Key; //Get the event ID

        Mumble.Event _eve = new Mumble.Event
        {
            title = titleInput.text,
            description = descInput.text,
            countdown_time = time,
            event_id = key,
        };

        string output = JsonConvert.SerializeObject(_eve);
        GameManager.instance.dbReference.Child("events").Child(key).SetRawJsonValueAsync(output).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                //Add event to active list of events (Only if the event uploaded successfully)
                GameManager.instance.dbReference.Child("event_list").Push().SetValueAsync(key);
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

        StorageReference uploadRef = GameManager.instance.stReference.Child("uploads/" + id + ".jpeg");

        uploadRef.PutBytesAsync(imageData, newMetaData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                GameManager.instance.infoDisplay.DisplayMessage("Image Uploaded Succesfully");
            }
        }
        );
    }

    bool CheckIfValid()
    {
        if (titleInput.text == "" || titleInput.text == null)
        {
            GameManager.instance.infoDisplay.DisplayError("No title has been set!");
            return false; //It is not valid
        }

        if (imageData == null)
        {
            GameManager.instance.infoDisplay.DisplayError("No image has been selected");
            return false; //It is not valid
        }
        return true;//It is valid
    }

    private void UpdateDisplayImage()
    {
        //Disable Placeholders
        imagePlaceholder.SetActive(false);

        //Set display Image
        Texture2D texture = new Texture2D(2, 2);
        ImageConversion.LoadImage(texture, imageData);

        displayImage.color = Color.white;
        displayImage.sprite = Sprite.Create(texture,
                                new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
                                Vector2.zero);
    }

    /* #region Timer
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
    */

    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
    }
}