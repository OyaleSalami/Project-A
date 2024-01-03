using Firebase.Extensions;
using Firebase.Storage;
using Mumble;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UploadManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputField descInput;

    [Header("Display Image")]
    [SerializeField] Image displayImage;
    [SerializeField] GameObject imageText;
    [SerializeField] GameObject imagePlaceholder;

    private byte[] imageData; //The bytes of the image to be uploaded

    void Start()
    {
        //Check if firebase is initialiazed
        if (GameManager.instance.isFirebaseReady)
        {
            //Good to go
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

    public void SelectFile()
    {
        string pngType = NativeFilePicker.ConvertExtensionToFileType("png");
        string jpgType = NativeFilePicker.ConvertExtensionToFileType("jpg");

        RequestPermissionAsynchronously(); //Request for file permissions on android

        //Select the Image //It calls "CheckImage" when it is done!
        NativeFilePicker.PickFile(CheckImage, pngType, jpgType);
    }

    private void CheckImage(string path)
    {
        if (path == null) //No Image was picked
        {
            return; //Exit the function
        }
        else //A file was picked
        {
            Texture2D texture = new Texture2D(2, 2); //Create a dummy texture
            imageData = File.ReadAllBytes(path); //Read the file from the path

            //Try converting the bytes to a texture
            if (ImageConversion.LoadImage(texture, imageData) != true)
            {
                //Invalid Image
                imageData = null; //Clear the image data that was loaded
                return;
            }
            else
            {
                if (imageData.Length > 7000000)
                {
                    //Image is greater than 7mb
                    imageData = null; //Clear the image data that was loaded
                    return;
                }
                else
                {
                    //Valid Image //Load Image Bytes //Safe Size
                    UpdateDisplayImage();
                }
            }
        }
    }

    private void UpdateDisplayImage()
    {
        imagePlaceholder.SetActive(false); //Disable Placeholders

        Texture2D texture = new Texture2D(2, 2); //Dummy texture
        ImageConversion.LoadImage(texture, imageData); //Load the image from imageData into a texture

        //Display the selected image
        displayImage.color = Color.white;
        displayImage.sprite = Sprite.Create(texture,
                                new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
                                Vector2.zero);
        displayImage.preserveAspect = true;
    }

    public void UploadEvent()
    {
        if (imageData == null)
        {
            return; //Exit, Do not upload (The Event is not valid/No image was selected)
        }

        //Generate the post ID
        string key = GameManager.instance.dbReference.Child("posts").Push().Key;

        //Creating a temporary post object
        Post _temp = new Post
        {
            description = descInput.text,
            postId = key,
        };

        //Convert the object to a Json string
        string output = JsonConvert.SerializeObject(_temp);

        GameManager.instance.dbReference.Child("posts").Child(key).SetRawJsonValueAsync(output).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                //Add event to active list of events (Only if the event uploaded successfully)
                GameManager.instance.dbReference.Child("post_list").Push().SetValueAsync(key);
            }
        }
        );

        //Upload Image
        UploadImageToBucket(key);

        //TODO: Add post to the user's profile(db)
    }

    void UploadImageToBucket(string id)
    {
        //Create the "image" metadata type for the file to be uploaded
        var newMetaData = new MetadataChange();
        newMetaData.ContentType = "image/jpeg";

        //Get the upload reference for the file
        StorageReference uploadRef = GameManager.instance.stReference.Child("uploads/" + id + ".jpeg");

        //Upload it asynchronously
        uploadRef.PutBytesAsync(imageData, newMetaData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                //Image succesfully uploaded
            }
        }
        );
    }

    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
    }
}