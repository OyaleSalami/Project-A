using Firebase.Firestore;
public class NetworkObjects
{

}

namespace Mumble
{
    [FirestoreData]
    public struct Event
    {
        [FirestoreProperty] public string name { get; set; }
        [FirestoreProperty] public string eventId { get; set; }
        [FirestoreProperty] public string description { get; set; }
        [FirestoreProperty] public int countdown_time { get; set; }
        [FirestoreProperty] public int times_played { get; set; }
        [FirestoreProperty] public int times_won { get; set; }
    }

    [FirestoreData]
    public struct PlayerProfile
    {
        [FirestoreProperty] public string name { get; set; }
        [FirestoreProperty] public string[] events_uploaded { get; set; }
        [FirestoreProperty] public int points { get; set; }
        [FirestoreProperty] public int no_events_uploaded { get; set; }
    }

    [FirestoreData]
    public struct Events
    {
        [FirestoreProperty] public string[] active_events { get; set; }
        [FirestoreProperty] public string[] all_events { get; set; }
        [FirestoreProperty] public int active_events_no { get; set; }
    }
}



