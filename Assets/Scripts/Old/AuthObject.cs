using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;

public class AuthObject : MonoBehaviour
{
    public RawImage image;
    FirebaseStorage storage;
    StorageReference storageReference;

    IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if(request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error Downloading the Image!");
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();

        //Initialize storage reference
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com");

        //Get image reference
        StorageReference imageReference = storageReference.Child("DALL·E 2023-05-04 12.05.19 - Murf written as text in  pixel art.png");

        //Get download link for file
        imageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
            {
                if(!task.IsFaulted && !task.IsCanceled)
                {
                    //StartCoroutine(LoadImage(Convert.ToString(task.Result)));
                }
                else
                {
                    Debug.Log("Error downloading image!" + task.Exception);
                }
            }
        );

        //StartCoroutine(LoadImage("https://firebasestorage.googleapis.com/v0/b/mumble-ccd73.appspot.com/o/DALL%C2%B7E%202023-05-04%2012.05.19%20-%20Murf%20written%20as%20text%20in%20%20pixel%20art.png?alt=media&token=9b8bb180-8586-435c-91d7-3359cb4f738f"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
