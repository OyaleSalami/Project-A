using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mumble;

public class CommentManager : MonoBehaviour
{
    public List<string> comments; //List of comment IDs
    public List<GameObject> commentObjects; //Reference to the comment object

    [SerializeField] GameObject commentAnchor;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] GameObject prevComment;
    [SerializeField] RectTransform scrollViewContentHolder;

    void Update()
    {
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
    public void AddComment(string postID, string comment)
    {
        Comment tempComment = new Comment();

        //TODO: Push the comment to the database
        string key = GameManager.instance.dbReference.Child("events").Push().Key; //Get the event ID
    }

    public void ClearComments()
    {

    }
}
