using Firebase.Database;
using Firebase.Extensions;
using Mumble;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentObject : MonoBehaviour
{
    /// <summary>The Unique ID For This Comment</summary>
    public string commentId;

    /// <summary>The Comment Descriptor</summary>
    public Comment comment;


    [Header("UI")]
    [SerializeField] Text displayName;
    [SerializeField] Text commentText;


    public void Start()
    {
        if (!string.IsNullOrEmpty(commentId))
        {
            LoadComment();
        }
    }

    public void UpdateUI()
    {
        commentText.text = comment.comment;
        displayName.text = "@" + comment.displayName;
    }

    public void LoadComment()
    {
        //Load the post from the database
        GameManager.instance.dbReference.Child("comments").Child(commentId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Do something with snapshot...
                Dictionary<string, object> dict = (Dictionary<string, object>)snapshot.Value;

                comment = new Comment(dict);
            }
        });
    }

}
