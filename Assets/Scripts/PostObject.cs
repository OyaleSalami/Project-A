using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using Mumble;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PostObject : MonoBehaviour
{
    public string id; //The post id
    public Post thisPost; //The description for this post

    [Header("UI")]
    [SerializeField] Image image; //Reference to the image display
    [SerializeField] GameObject clickButton;
    [SerializeField] GameObject reloadButton;
    [SerializeField] GameObject commentPanel;

    [Space(20)][Header("Post UI")]
    [SerializeField] Text username;
    [SerializeField] Text seenNumber;
    [SerializeField] Text likesNumber;
    [SerializeField] Text commentsNumber;
    [SerializeField] Text savesNumber;

    [SerializeField] GameObject likeImageFull;
    [SerializeField] GameObject savedImageFull;
    [SerializeField] GameObject commentImageFull;

    bool loaded = false;

    void Start()
    {
        commentPanel = GameObject.FindGameObjectWithTag("comments");
    }

    public void UpdateDisplay()
    {
        if (loaded == true)
        {
            seenNumber.text = thisPost.seen.ToString();
            likesNumber.text = thisPost.likes.ToString();
            savesNumber.text = thisPost.saves.ToString();
            commentsNumber.text = thisPost.comments.ToString();
            username.text = "   @" + thisPost.username.ToString();

            //TODO: Check if the user has liked, saved or commented on the post
            /*
            if(liked == true)
            {
                likeImageFull.setActive(true);
            }

            if(comment == true)
            {
                savedImageFull.setActive(true);
            }

            if(saved == true)
            {
                commentImageFull.setActive(true);
            }
            */
        }
    }

    public void LoadEvent(string _id)
    {
        id = _id;
        //Load the post from the database
        GameManager.instance.dbReference.Child("posts").Child(_id).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                    reloadButton.SetActive(true);
                    clickButton.SetActive(false);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    // Do something with snapshot...
                    Dictionary<string, object> dict = (Dictionary<string, object>)snapshot.Value;

                    thisPost = new Post(dict);
                    LoadImage();
                }
            });
    }

    //Download image from firebase to display
    void LoadImage()
    {
        //Get image reference
        StorageReference imageReference = GameManager.instance.stReference.Child("uploads/" + id + ".jpeg");

        //Get the download link for file
        imageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImageFromCloud(Convert.ToString(task.Result)));
            }
            else
            {
                Debug.Log("Error downloading image!" + task.Exception);
                reloadButton.SetActive(true);
                clickButton.SetActive(false);
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
            reloadButton.SetActive(true);
            clickButton.SetActive(false);
        }
        else
        {
            loaded = true;
            reloadButton.SetActive(false);
            clickButton.SetActive(true);

            image.color = Color.white;
            //image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            //TODO: Resize image here
            Texture2D text = ((DownloadHandlerTexture)request.downloadHandler).texture;
            image.sprite = Sprite.Create(text, new Rect(Vector2.zero, new Vector2(text.width, text.height)),
                            Vector2.zero);
        }
    }

    public void CheckComments()
    {
        //TODO: Check the comments section
        commentPanel.SetActive(true);
    }

    public void CheckSaved()
    {
        //TODO: Check the saved events
    }

    public void LikePost()
    {
        //TODO: Like the post
    }

    public void SavePost()
    {
        //TODO: Save the post to the user profile
    }
}