using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventMenu : MonoBehaviour
{
    [Header("File Paths")]
    [SerializeField] string path = ""; //Path to where the images are stored
    string dataPath; //Path to where the images will be stored (Platform Independent)

    [Header("Loaded Textures")]
    [SerializeField] int imagesLoaded = 0; //Number of images that have been loaded (Succesfully)
    [SerializeField] List<Texture2D> textures; //The list of the converted images (Temp)

    [Header("Displayed Image")]
    [SerializeField] Image displayImage; //The image currently displayed
    [SerializeField] int selectedImageIndex = 0; //Its index in the list
    [SerializeField] Image completedImage; //The completed image on the game board (Game scene)
    [SerializeField] Material mumbleTex; //Prefab for the image for the game board

    // Start is called before the first frame update
    void Start()
    {
        dataPath = Application.dataPath;
        dataPath += "/Mumble";

        GetImages();
    }

    public void GetImages()
    {
        //Get all the picture files in the directory
        List<string> filePaths = new List<string>();
        filePaths.AddRange(Directory.GetFiles(path, "*.png")); //Add all the png images!
        filePaths.AddRange(Directory.GetFiles(path, "*.jpg")); //Add all the jpg images!

        //Resize the texture list to the no of files found
        textures = new List<Texture2D>(filePaths.Count);

        for (int i = 0; i < filePaths.Count; i++)
        {
            try
            {
                //Read the image as a series of bytes
                byte[] imageData = File.ReadAllBytes(filePaths[i]);

                textures.Add(new Texture2D(2,2));
                Debug.Log(textures.Count);

                //Convert the series of bytes to a texture
                if (ImageConversion.LoadImage(textures[i], imageData) == true)
                {
                    Debug.Log("Error Converting The Image!");
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error Loading Image (" + filePaths[i] + "): " +  e);
            }
        }
        imagesLoaded = textures.Count;
        UpdateImage(); //Update the UI
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void SelectEvent()
    {
        if (imagesLoaded <= 0)
        {
            //TODO: Do something else
            return;
        }
        else
        {
            //Set the selected image as Image/Completed_Image on the game board
            mumbleTex.mainTexture = textures[selectedImageIndex];

            completedImage.sprite = Sprite.Create(textures[selectedImageIndex],
               new Rect(Vector2.zero, new Vector2(textures[selectedImageIndex].width, textures[selectedImageIndex].height)),
               Vector2.zero);

            SceneManager.LoadScene("Game Scene", LoadSceneMode.Single);
        }
    }

    public void NextEvent()
    {
        //Clamp the Selected Image value
        if (selectedImageIndex + 1 >= imagesLoaded)
        {
            selectedImageIndex = imagesLoaded - 1;
        }
        else
        {
            selectedImageIndex += 1;
        }
        UpdateImage();
    }

    public void PreviousEvent()
    {
        if (selectedImageIndex <= 0)
        {
            selectedImageIndex = 0;
        }
        else
        {
            selectedImageIndex = selectedImageIndex - 1;
        }
        UpdateImage();
    }

    public void UpdateImage()
    {
        if (imagesLoaded <= 0)
        {
            //TODO: No images available
            return;
        }
        else
        {
            displayImage.sprite = Sprite.Create(textures[selectedImageIndex],
               new Rect(Vector2.zero, new Vector2(textures[selectedImageIndex].width, textures[selectedImageIndex].height)),
               Vector2.zero);
        }
    }

    public void Upload()
    {
        string filePath = ""; //Path to the image to be uploaded
        try
        {
            filePath = EditorUtility.OpenFilePanel("Choose Image To Upload", "", "");
        }
        catch (Exception e)
        {
            Debug.LogError("Error Uploading File (" + filePath + "): " + e);
            filePath = "";
            return;
        }

        if (filePath != "" && filePath != null)
        {
            Debug.Log("Uploaded a New Event: " + filePath);
            //TODO: Move unto the payment page and stuff
        }
        else
        {
            Debug.Log("Error Uploading New Event: " + filePath);
        }
    }
}
