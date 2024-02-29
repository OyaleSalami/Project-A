using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Mumble
{
    [System.Serializable] public class Comment
    {
        /// <summary>The Display Name Of The Poster</summary>
        public string displayName;

        /// <summary>The Comment Made By The Poster</summary>
        public string comment;

        /// <summary>The Number Of Likes On The Comment</summary>
        public int likesCount;

        public Comment()
        {
            likesCount = 0;
        }

        public Comment(Dictionary<string, object> _comment)
        {
            try
            {
                displayName = _comment["displayName"].ToString();
                comment     = _comment["comment"].ToString();
                likesCount  = int.Parse(_comment["likesCount"].ToString());
            }
            catch(Exception e)
            {
                Debug.Log("Error creating comment: " + e);
            }
        }

        /// <summary>Returns This Object As A Json String</summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [System.Serializable] public class Post
    {
        /// <summary>The Unique ID For This Post (Identifies post and comment sections)</summary>
        public string postId;
        
        /// <summary>User ID Of The Poster</summary>
        public string userId;

        /// <summary>The Display Name Of The USer</summary>
        public string displayName;

        /// <summary>The Description Of The Post</summary>
        public string description;

        /// <summary>View Count</summary>
        public int viewCount;

        /// <summary>Likes Count</summary>
        public int likesCount;

        /// <summary>Comment Count</summary>
        public int commentsCount;

        /// <summary>Save Count</summary>
        public int savesCount;


        public Post()
        {
            viewCount     = 0;
            likesCount    = 0;
            commentsCount = 0;
            savesCount    = 0;
        }

        public Post(Dictionary<string, object> _post)
        {
            try
            {
                postId      = _post["postId"].ToString();
                userId      = _post["userId"].ToString();
                displayName = _post["displayName"].ToString();
                description = _post["description"].ToString();

                viewCount     = int.Parse(_post["viewCount"].ToString());
                likesCount    = int.Parse(_post["likesCount"].ToString());
                savesCount    = int.Parse(_post["savesCount"].ToString());
                commentsCount = int.Parse(_post["commentsCount"].ToString());
            }
            catch (Exception e)
            {
                Debug.Log("Error Creating Post: " + e);
            }
        }

        /// <summary>Returns the object as a JSon string</summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [System.Serializable] public class User
    {
        /// <summary>The User's Unique Id</summary>
        public string userId;

        ///<summary> User's Display Name </summary>
        public string displayName; 

        /// <summary>Number Of Posts Made By The User</summary>
        public int postCount;

        public User(Dictionary<string, object> user)
        {
            try
            {
                userId      = user["userId"].ToString();
                displayName = user["displayName"].ToString();
                postCount   = int.Parse(user["postCount"].ToString());
            }
            catch (Exception e)
            {
                Debug.Log("Invalid dictionary: " + e);
            }
        }

        /// <summary>Returns the object as a JSon string</summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}