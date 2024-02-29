using Firebase.Extensions;
using Mumble;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class CommentManager : MonoBehaviour
{
    /// <summary>Unique ID Of The Post That Has The Comments</summary>
    public string postId;

    /// <summary>List Of The Comment IDs For This Post</summary>
    public List<string> comments;

    [Header("UI")]
    [SerializeField] GameObject commentAnchor;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] GameObject prevComment;
    [SerializeField] RectTransform scrollViewContentHolder;
    [SerializeField] List<GameObject> commentObjects; //Reference to the comment objects

    void Update()
    {
        //TODO: Change the button mapping to register the back button
        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadAllComments()
    {
        foreach (var comm in comments)
        {
            CreateCommentLine(comm);
        }
    }

    public void CreateCommentLine(string id)
    {
        float posDifference = commentPrefab.GetComponent<RectTransform>().rect.height + 5;

        //Create a new event and position it properly
        GameObject temp = Instantiate(commentPrefab, commentAnchor.transform);
        temp.transform.position = prevComment.transform.position - new Vector3(0, posDifference, 0);
        scrollViewContentHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollViewContentHolder.rect.height + posDifference);

        prevComment = temp;
        temp.GetComponent<CommentObject>().commentId = id;
        temp.GetComponent<CommentObject>().LoadComment();
    }

    //Add a comment to a post
    public void AddNewComment(string _comment)
    {
        //Create a temporary object to store the comment
        Comment temp = new()
        {
            comment = _comment,
            displayName = GameManager.instance.user.DisplayName,
            likesCount = 0
        };

        //Create And Get the unique ID for the new comment
        string key = GameManager.instance.dbReference.Child("comments").Child(postId).Push().Key;
        string output = temp.ToJson();

        GameManager.instance.dbReference.Child("comments").Child(postId).Child(key).SetRawJsonValueAsync(output).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                //Add event to active list of events (Only if the event uploaded successfully)
                GameManager.instance.dbReference.Child("posts").Child(postId).Child("comment_list").Push().SetValueAsync(key);
            }
        }
        );
    }
}
