using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Mumble
{
    [System.Serializable] public class Comment
    {
        public string username;
        public string comment;
        public int likes;

        public Comment()
        {

        }

        public Comment(string _comment, string _username, int _likes)
        {
            username = _username;
            comment = _comment;
            likes = _likes;
        }

        public Comment(Dictionary<string, object> _comment)
        {
            try
            {
                username = _comment["username"].ToString();
                comment = _comment["comment"].ToString();
                likes = int.Parse(_comment["likes"].ToString());
            }
            catch(Exception e)
            {
                Debug.Log("Error creating comment: " + e);
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [System.Serializable] public class Post
    {
        public int seen; //No of times the post was seen
        public int likes; //No of likes per post
        public int comments; //No of comments
        public int saves; //No of saves
        public string postId; //Post ID (Identifies post and comment sections)
        public string description; //Description of the post
        public string username; //Username of the person that posted it

        //Creating a blank post object
        public Post()
        {
            seen = 0;
            likes = 0;
            comments = 0;
            saves = 0;
            //TODO: username = GameManager.instance.user.DisplayName; 
        }

        //Re-Create a post object from a dictionary
        public Post(Dictionary<string, object> _post)
        {
            try
            {
                seen = int.Parse(_post["posts"].ToString());
                likes = int.Parse(_post["likes"].ToString());
                comments = int.Parse(_post["comments"].ToString());
                saves = int.Parse(_post["saves"].ToString());
                postId = _post["postId"].ToString();
                description = _post["description"].ToString();
                username = _post["username"].ToString();
            }
            catch (Exception e)
            {
                Debug.Log("Error Creating Post: " + e);
            }
        }

        //Create a post object from a Json file/string
        public Post(string json)
        {
            try
            {
                Dictionary<string, object> _post = (Dictionary<string, object>)JsonConvert.DeserializeObject(json);
            }
            catch(Exception e)
            {
                Debug.Log("Error Creating Post: " + e);
            }
        }

        //Return the object as a Json string
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [System.Serializable] [System.Obsolete]
    public class Event
    {
        public Event()
        {

        }

        public Event(Dictionary<string, object> dict)
        {
            try
            {
                title = dict["title"].ToString();
                event_id = dict["event_id"].ToString();
                description = dict["description"].ToString();
                countdown_time = int.Parse(dict["countdown_time"].ToString());
                times_played = int.Parse(dict["times_played"].ToString());
                times_won = int.Parse(dict["times_won"].ToString());
            }
            catch (Exception e)
            {
                Debug.Log("Invalid dictionary: " + e);
            }
        }

        /// <summary>The title of the game event</summary>
        public string title { get; set; }
        /// <summary>The Id for that event</summary>
        public string event_id { get; set; }
        /// <summary>The description of the event</summary>
        public string description { get; set; }
        /// <summary>The timer for the event</summary>
        public int countdown_time { get; set; }
        /// <summary>The no of times played</summary>
        public int times_played { get; }
        /// <summary>The no of times won</summary>
        public int times_won { get; }
    }

    [System.Serializable] public class PlayerProfile
    {
        public string username; //Player Username
        public string phonenumber; //Player's phonenumber
        public int postcount; //No of posts by the player
        public int points; //Points the player has
        public int gamesplayed; //No of games the player has played
        public int gameswon; //No of games the player has won

        public string[] posts; //List of posts uploaded by the player (their IDs)
        public string[] saved; //List of posts saved by the player (their IDs)
        public string[] liked; //List of posts like by the player (their IDs)

        public PlayerProfile(Dictionary<string, object> prof)
        {
            try
            {
                username = prof["name"].ToString();
                phonenumber = prof["phonenumber"].ToString();
                postcount = int.Parse(prof["postcount"].ToString());
                points = int.Parse(prof["points"].ToString());
                gamesplayed = int.Parse(prof["games_played"].ToString());
                gameswon = int.Parse(prof["games_won"].ToString());

                /*
                 * TODO: Get the lists for the other details
                 * posts = 
                 * saved = 
                 * liked = 
                */
            }
            catch (Exception e)
            {
                Debug.Log("Invalid dictionary: " + e);
            }
        }

        [System.Obsolete] public PlayerProfile(string json)
        {
            JsonConvert.DeserializeObject(json);
        }
    }
}