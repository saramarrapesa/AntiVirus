using System.Collections;
using UnityEngine;
using LootLocker.Requests;


public class PlayerManager : MonoBehaviour
{
    public Leaderboard leaderboard;




    // Start is called before the first frame update
    public void StartGame()
    {
        
        StartCoroutine(SetupRoutine());
    }

    #region Guest Session
    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }
    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    #endregion

}



