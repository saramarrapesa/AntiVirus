using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject leaderboardPanel;
    public ScoreManager scoreManager;
    public Leaderboard leaderboard;
    

    private bool submitSent = false;

    // Update is called once per frame
    void Update()
    {
      if(GameObject.FindGameObjectWithTag("Player") == null && !submitSent)
        {
            gameOverPanel.SetActive(true);
            scoreManager = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
            StartCoroutine(UploadAndShow());
            submitSent = true;
        } 
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator UploadAndShow()
    {
        yield return leaderboard.SubmitScoreRoutine(scoreManager.scoreDaPassare);
        yield return leaderboard.FetchTopHighscoresRoutine();
    }

    public void BackToGameOver()
    {
        gameOverPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
    }
}
