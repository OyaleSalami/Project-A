using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EventLoader : MonoBehaviour
{
    [Header("UI/UX")]
    ///<summary>The line we are on</summary>
    [SerializeField] int lineIndex;
    ///<summary>The list of all event lines</summary>
    [SerializeField] List<GameObject> eventLines; 

    [SerializeField] GameObject eventLinePrefab; //Event Line Prefab
    [SerializeField] GameObject scrollViewContent;//Spawn Transform
    [SerializeField] RectTransform scrollViewContentHolder;//Content Transform

    void Start()
    {
        GetEventsFromCloud();
    }

    public void GetEventsFromCloud()
    {
        DatabaseManager.GetActiveEvents();
    }

    /// <summary>Upload a new event with custom Images</summary>
    /// <param name="path">Path to the file to upload</param>
    public void UploadEvent(string path)
    {
        byte[] imageData;
        try
        {
            //Read the image as a series of bytes
            imageData = File.ReadAllBytes(path);

            //TODO: Do a size check for the file

            //Check to see if it is a valid Image
            if (ImageConversion.LoadImage(new Texture2D(2, 2), imageData) == true)
            {
                DatabaseManager.UploadEvent(imageData);
            }
            else
            {
                Debug.Log("Error Invalid Message!");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error Loading Image (" + path + "): " + e);
        }
    }

    public void SelectFile()
    {
        string filePath = ""; //Path to the image to be uploaded
        try
        {
            //Get the file to upload
            filePath = EditorUtility.OpenFilePanel("Choose Image To Upload", "", "");
        }
        catch (Exception e)
        {
            Debug.LogError("Error Uploading File (" + filePath + "): " + e);
            filePath = "";
            //TODO: Show error message!
            return;
        }

        if (filePath != "" && filePath != null)
        {
            Debug.Log("Uploaded a New Event: " + filePath);
            //TODO: Move unto the payment page and stuff

            //DEMO: For testing
            UploadEvent(filePath);
        }
        else
        {
            //TODO: Show error message!
            Debug.Log("Error Uploading New Event: " + filePath);
        }
    }

    #region UI/UX
    void AddEvent()
    {
        foreach (var id in DatabaseManager.activeEvents)
        {
            if (eventLines[lineIndex].GetComponent<EventLine>().index >= 2)
            {
                //Event Line is full, Create a new one and continue adding events
                CreateNewEventLine();
                lineIndex++;
                eventLines[lineIndex].GetComponent<EventLine>().AddEvent(id);
            }
            else
            {
                //Event Line still has space, add more events
                eventLines[lineIndex].GetComponent<EventLine>().AddEvent(id);
            }
        }
    }

    public void CreateNewEventLine()
    {
        //Create a new event line and position it properly
        GameObject temp = Instantiate(eventLinePrefab, scrollViewContent.transform);
        temp.transform.position = eventLines[eventLines.Count - 1].transform.position - new Vector3(0, 430, 0);

        scrollViewContentHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollViewContentHolder.rect.height + 430);
        eventLines.Add(temp);
    }
    #endregion
}
