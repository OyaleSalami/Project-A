using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;

public class DatabaseManager : MonoBehaviour
{
    FirebaseStorage storage;
    DatabaseReference reference;
    static StorageReference storageReference;

    //STATIC PROPERTIES
    /// <summary>A list of all the IDs for active game events</summary>
    public static List<string> activeEvents;
    //STATIC PROPERTIES

    void Start()
    {
        //Databsase Initialization
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //Storage Bucket Initialization
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com");
    }

    public static void UploadEvent(byte[] imageData)
    {
        //TODO: Add to active events list

        //TODO: Add to the player's profile

        //Upload the image to the bucket
        var newMetaData = new MetadataChange();
        newMetaData.ContentType = "image/jpeg";

        //Create a reference to where the file needs to be uploaded
        StorageReference uploadRef = storageReference.Child("events/" + DateTime.Now + ".jpeg");

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
    }
}
