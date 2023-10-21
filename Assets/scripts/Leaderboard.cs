using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using TMPro;


public class Leaderboard : MonoBehaviour
{
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;
    public GameObject leaderboardPanel;
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        Debug.Log("current_player:" + playerID);
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, "globalHighscore", (response) =>
        {

            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList("globalHighscore", 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i< members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed" + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void GetLeaderboard()
    {
        gameOverPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }
}
