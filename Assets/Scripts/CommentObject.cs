using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mumble;

public class CommentObject : MonoBehaviour
{
    public string id;
    public Comment thisComment;

    [Header("UI")]
    [SerializeField] Text username;
    [SerializeField] Text comment;

    
    public void LoadComment(string _id)
    {
        //TODO: Load the comment from the db
    }

    public void UpdateDisplay()
    {
        username.text = thisComment.username;
        comment.text = thisComment.comment;
    }
}
