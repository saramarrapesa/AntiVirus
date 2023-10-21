using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text scoreText;
    public float score;
    public int scoreDaPassare;

    
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            score += 1 * Time.deltaTime;
            scoreDaPassare = (int)score;
            scoreText.text = ((int)score).ToString();
        }

    }

   /* public int getScore()
    {
        return (int)score;
    }*/

}
