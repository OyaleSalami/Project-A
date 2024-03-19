using Firebase.Extensions;
using Firebase.Storage;
using Mumble;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
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
        if (GameManager.instance.isFirebaseReady == false) //Check if firebase is initialiazed
        {
            if (GameManager.instance.Initialize() != true) //Try to initialize it
            {
                throw new Exception("Unable to initialize the Game Manager");
            }
        }
    }

    /// <summary>Goes back to the main scene</summary>
    public void GoToHome()
    {
        LoadScript.LoadScene(1f, "Main");
    }

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
                //Invalid Image //Clear the image data that was loaded
                imageData = null; return;
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

    /// <summary>Set's the Image display to the selected image</summary>
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
            userId = GameManager.instance.user.UserId,
            displayName = GameManager.instance.user.DisplayName
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
                //Add event to active list of events
                GameManager.instance.dbReference.Child("post_list").Push().SetValueAsync(key);

                //Add event to users list of events
                GameManager.instance.dbReference.Child("users").Child(_temp.userId).Child("posts").Push().SetValueAsync(key);
            }
        }
        );

        //Upload Image
        UploadImageToBucket(key);
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

    /// <summary>Request permission to read a file on mobile</summary>
    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
    }
}