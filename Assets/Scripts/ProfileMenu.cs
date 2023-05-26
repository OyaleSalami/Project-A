using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileMenu : MonoBehaviour
{
    [SerializeField] Text playerName;
    [SerializeField] Text playerBalance;
    [SerializeField] Image playerDp;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayerName();
        GetPlayerBalance();
        GetPlayerDp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetPlayerName()
    {
        //Update player name
    }
    void GetPlayerBalance()
    {
        //Update player balance
    }
    void GetPlayerDp()
    {

    }

    void SetPlayerDetails()
    {

    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void Withdraw()
    {
        Debug.Log("Withdraw Money!");
    }

    public void ConnectPlayGames()
    {
        Debug.Log("Connect to Google Play Games!");
    }
}
