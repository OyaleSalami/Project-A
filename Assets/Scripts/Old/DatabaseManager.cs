using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;

public class DatabaseManager : MonoBehaviour
{
    FirebaseStorage storage;

    /// <summary>Reference to the realtime database</summary>
    static DatabaseReference dbReference;

    /// <summary>Reference to the storage bucket</summary>
    static StorageReference storageReference;

    //STATIC PROPERTIES
    /// <summary>A list of all the IDs for active game events</summary>
    public static List<string> activeEvents;
    //STATIC PROPERTIES

    void Start()
    {
        //Databsase Initialization
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        //Storage Bucket Initialization
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com");
    }

    public static void UploadEvent(ref Mumble.Post _event, byte[] imageData)
    {
        _event.postId = DateTime.Now.ToUniversalTime().ToString();

        string json = JsonUtility.ToJson(_event);

        //TODO: Add to active events list
        dbReference.Child("active_events").SetRawJsonValueAsync(json);

        //TODO: Check if you are signed in properly

        //TODO: Add to the player's profile
        //dbReference.Child("users").Child(ProfileManager.userID).Child("events").SetValueAsync(_event.event_id);

        //Upload the image to the bucket
        var newMetaData = new MetadataChange
        {
            ContentType = "image/jpeg"
        };

        //Create a reference to where the file needs to be uploaded
        StorageReference uploadRef = storageReference.Child(_event.postId + ".jpeg");

        uploadRef.PutBytesAsync(imageData, newMetaData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                Debug.Log("File Uploaded Successfully");
            }
        }
        );
    }

    public static void GetActiveEvents()
    {
        //Get the list of active events and store it in "activeEvents"
        dbReference.Child("active_events").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    //StartCoroutine(LoadImage(Convert.ToString(task.Result)));
                }
                else
                {
                    Debug.Log("Error getting active events: " + task.Exception);
                }
            }
        );
    }
}
