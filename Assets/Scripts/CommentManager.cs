using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Mumble;
using Firebase.Extensions;

public class CommentManager : MonoBehaviour
{
    public string postId; //Id of the post that has the comments
    public List<string> comments; //List of comment IDs
    public List<GameObject> commentObjects; //Reference to the comment objects

    [Header("UI")]
    [SerializeField] GameObject commentAnchor;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] GameObject prevComment;
    [SerializeField] RectTransform scrollViewContentHolder;

    void Update()
    { 
        //TODO: Change the button mapping to register the back button
        if(Input.GetKey(KeyCode.Escape))
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
        temp.GetComponent<CommentObject>().LoadComment(id);
    }

    //Add a comment to a post
    public void AddNewComment(string comment)
    {
        Comment temp = new Comment(comment, GameManager.instance.user.DisplayName, 0);

        //TODO: Push the comment to the database
        string key = GameManager.instance.dbReference.Child("comments").Child(postId).Push().Key; //Get the event ID
        string output = JsonConvert.SerializeObject(temp);

        GameManager.instance.dbReference.Child("events").Child(postId).Child(key).SetRawJsonValueAsync(output).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                //Add event to active list of events (Only if the event uploaded successfully)
                GameManager.instance.dbReference.Child("event_list").Push().SetValueAsync(key);
            }
        }
        );
    }
}
