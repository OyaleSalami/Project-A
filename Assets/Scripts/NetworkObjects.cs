using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mumble
{
    [System.Serializable]
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
                countdown_time = (int)dict["countdown_time"];
                times_played = (int)dict["times_played"];
                times_won = (int)dict["times_won"];
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

    [System.Serializable]
    public class PlayerProfile
    {
        public PlayerProfile(Dictionary<string, object> dict)
        {
            try
            {
                userName = dict["name"].ToString();
                number = dict["number"].ToString();
                points = dict["points"].ToString();
                games_played = dict["games_played"].ToString();
                games_won = dict["games_won"].ToString();
                games_uploaded = dict["games_uploaded"].ToString();
            }
            catch (Exception e)
            {
                Debug.Log("Invalid dictionary: " + e);
            }
        }

        /// <summary>The player's display name</summary>
        public string userName { get; set; }
        /// <summary>The player's number</summary>
        public string number { get; set; }
        /// <summary>The player's current points</summary>
        public string points { get; set; }
        /// <summary>The number of games played by the user</summary>
        public string games_played { get; set; }
        /// <summary>The number of games won by the user</summary>
        public string games_won { get; set; }
        /// <summary>The number of games uploaded by the user</summary>
        public string games_uploaded { get; set; }
        /// <summary>List of the event hashes</summary>
        public string[] events { get; set; }
    }


}