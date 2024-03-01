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
    /// <summary>Unique ID Of This Post</summary>
    public string postId;

    /// <summary>The Post Descriptor</summary>
    public Post post;

    [Header("UI")]
    [SerializeField] Image displayImage;
    [SerializeField] GameObject errorPanel;

    [Space(20)]
    [Header("Post UI")]
    [SerializeField] Text displayName;
    [SerializeField] Text viewCount;
    [SerializeField] Text savesCount;
    [SerializeField] Text likesCount;
    [SerializeField] Text commentsCount;

    private bool loaded = false;

    void Start()
    {
        //commentPanel = GameObject.FindGameObjectWithTag("comments");

        //Load The Event If There Is A Given Post ID
        if(!string.IsNullOrEmpty(postId))
        {
            LoadPost();
        }
    }

    public void UpdateUI()
    {
        viewCount.text     = TransformCount(post.viewCount);
        likesCount.text    = TransformCount(post.likesCount);
        savesCount.text    = TransformCount(post.savesCount);
        commentsCount.text = TransformCount(post.commentsCount);
        displayName.text   = "   @" + post.displayName.ToString();
    }

    public void LoadPost()
    {
        //Load the post from the database
        GameManager.instance.dbReference.Child("posts").Child(postId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                errorPanel.SetActive(true);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Do something with snapshot...
                Dictionary<string, object> dict = (Dictionary<string, object>)snapshot.Value;

                post = new Post(dict);
                LoadImage();
            }
        });
    }

    void LoadImage()
    {
        //Get image reference
        StorageReference imageReference = GameManager.instance.stReference.Child("uploads/" + postId + ".jpeg");

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
                errorPanel.SetActive(true);
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
            errorPanel.SetActive(true);
        }
        else
        {
            loaded = true;
            displayImage.color = Color.white;

            //image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            //TODO: Resize image here
            Texture2D text = ((DownloadHandlerTexture)request.downloadHandler).texture;
            displayImage.sprite = Sprite.Create(text, new Rect(Vector2.zero, new Vector2(text.width, text.height)),
                            Vector2.zero);
            displayImage.preserveAspect = true;
        }
    }

    public void CheckComments()
    {
        //TODO: Check the comments section
    }

    public void LikePost()
    {
        //TODO: Like the post
    }

    public void SavePost()
    {
        //TODO: Save the post to the user profile
    }

    string TransformCount(int val)
    {
        if (val > 1000)
        {
            if (val > 1000000)
            {
                return (val / 1000000).ToString() + "m";
            }

            return (val / 1000).ToString() + "k";
        }

        return val.ToString();
    }

}