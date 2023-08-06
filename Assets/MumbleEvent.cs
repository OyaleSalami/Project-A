using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class MumbleEvent : MonoBehaviour
{
    public RawImage image; //Reference to the image display

    FirebaseStorage storage;
    StorageReference storageReference;

    public Mumble.Event thisEvent; //The description for this event

    void Start()
    {
        image = GetComponent<RawImage>();

        //Initialize storage reference
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com");
    }

    public void LoadImage()
    {
        //Download image and details from firebase and display

        //Get image reference
        StorageReference imageReference = storageReference.Child("/events/" + thisEvent.eventId + ".jpeg");

        //Get download link for file
        imageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImageFromCloud(Convert.ToString(task.Result)));
            }
            else
            {
                Debug.Log("Error downloading image!" + task.Exception);
            }
        }
        );
    }

    IEnumerator LoadImageFromCloud(string _url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(_url);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error Downloading the Image!");
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}
