using Firebase.Extensions;
using Mumble;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentManager : MonoBehaviour
{
    /// <summary>Unique ID Of The Post That Has The Comments</summary>
    public string postId;

    /// <summary>List Of The Comment IDs For This Post</summary>
    private List<string> comments;

    [Header("UI")]
    [SerializeField] GameObject commentAnchor;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] GameObject prevComment;
    [SerializeField] RectTransform scrollViewContentHolder;
    [SerializeField] List<GameObject> commentObjects; //Reference to the comment objects


    [Header("Comment Submission")]
    [SerializeField] InputField commentInput;

    void Update()
    {
        //TODO: Change the button mapping to register the back button
        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
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
    
    /// <summary>Creates a new comment under a particular post</summary>
    public void SubmitComment()
    {
        //Generate the comment ID
        string key = GameManager.instance.dbReference.Child("comments").Child(postId).Push().Key;

        //Creating a temporary comment object
        Comment _comment = new Comment
        {
            comment = commentInput.text,
            likesCount = 0,
            userId = GameManager.instance.user.UserId,
            displayName = GameManager.instance.user.DisplayName
        };

        //Convert the object to a Json string
        string output = _comment.ToJson();

        GameManager.instance.dbReference.Child("comments").Child(postId).Child(key).SetRawJsonValueAsync(output).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                //Add comment to active list of comments under a particular post
                GameManager.instance.dbReference.Child("comments_list").Child(postId).Push().SetValueAsync(key);
            }
        }
        );

        commentInput.text = "";
    }
}
