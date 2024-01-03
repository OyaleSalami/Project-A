using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mumble;
using UnityEngine.UI;

public class PostSummaryObject : MonoBehaviour
{
    public string postID;
    private Post thisPost;

    [SerializeField] Text likes;
    [SerializeField] Text comments;
    [SerializeField] Text seen;
    [SerializeField] Text saves;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UpdateUI()
    {
        likes.text = ConvertValues(thisPost.likes);
        comments.text = ConvertValues(thisPost.comments);
        seen.text = ConvertValues(thisPost.seen);
        saves.text = ConvertValues(thisPost.saves);
    }

    public void GetPostDetails()
    {
        GameManager.instance.dbReference.Child("posts").Child(postID).GetValueAsync()
            .ContinueWithOnMainThread(task =>
           {
               if (task.IsFaulted)
               {
                    // Handle the error...
                   Debug.LogError(task.Exception);
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

                   // Do something with snapshot...
                   Dictionary<string, object> _temp = (Dictionary<string, object>)snapshot.Value;
                   thisPost = new Post(_temp);
                   UpdateUI();
               }
           });
    }

    private string ConvertValues(int value)
    {
        if (value < 0)
        {
            value = 0;
            return value.ToString();
        }
        else if(value < 1000)
        {
            return value.ToString();
        }

        if(value < 1000000)
        {
            float val = value / 1000;
            return (val.ToString() + "K"); //eg 88.97K
        }
        else //Value is above 1M
        {
            float val = value / 1000000;
            return (val.ToString() + "M"); //eg 898.97M
        }
    }
}
