using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("Event Description")]
    [SerializeField] Text eventTitle;
    [SerializeField] Text eventDescription;
    [SerializeField] Image displayImage;

    [Header("Image Details")]
    [SerializeField] byte[] imageBytes;
    [SerializeField] string imagePath = "";
    [SerializeField] Texture2D texture;

    [Header("Timer")]
    [SerializeField] int time;
    [SerializeField] Text timerText;

    [SerializeField] GameObject dummyObject;

    void Start()
    {
        dummyObject.SetActive(true);

        //Reset Timer
        time = 5;
        UpdateTimeUI();

        //Reset Display Texts
        eventTitle.text = "";
        eventDescription.text = "";

        //Reset Images
        displayImage.sprite = null;

        texture = new Texture2D(2, 2);

        //TODO: Reset Image Bytes
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    #region Timer
    public void AddTime()
    {
        time += 1;
        if (time >= 59)
        {
            time = 59; //59 minutes is the highest
        }
        UpdateTimeUI();
    }

    public void ReduceTime()
    {
        time -= 1;
        if (time <= 5)
        {
            time = 5; //5 minutes is the lowest time
        }
        UpdateTimeUI();
    }

    public void UpdateTimeUI()
    {
        timerText.text = time.ToString() + ":00";
    }
    #endregion

    public bool CheckEventState()
    {
        if (eventTitle.text != "" && displayImage.sprite == null)
        {
            return false;
        }

        return true;
    }

    public void SelectImageToUpload()
    {
        string filePath = ""; //Path to the image to be uploaded

        try
        {
            //Get the file to upload
            //filePath = EditorUtility.OpenFilePanelWithFilters("Choose Image To Upload", "", new string[] { "Image files", "png,jpg,jpeg"});
        }
        catch (Exception e)
        {
            Debug.LogError("Error Uploading File (" + filePath + "): " + e);
            return; //Exit
        }

        if (filePath != "" && filePath != null)
        {
            byte[] imageData = File.ReadAllBytes(filePath); //Load Image Bytes

            if (ImageConversion.LoadImage(texture, imageData) != true)
            {
                Debug.Log("Invalid Image!");
            }
            else //Success
            {
                dummyObject.SetActive(false);
                Debug.Log("Uploaded Image: " + filePath);
                displayImage.sprite = Sprite.Create(texture,
                                new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
                                Vector2.zero);
                imageBytes = imageData;
            }
        }
        else
        {
            Debug.Log("Error Uploading Image: " + filePath);
        }
    }

    public void UploadEvent()
    {
        if (CheckEventState() == false) //Everything is not set up properly
        {
            return; //Exit 
            //TODO: Display error message
        }

        Mumble.Event _event = new()
        {
            //name = eventTitle.text,
            description = eventDescription.text,
            countdown_time = time
        };

        DatabaseManager.UploadEvent(ref _event, imageBytes);
    }
}
