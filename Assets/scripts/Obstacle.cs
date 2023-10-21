using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Border")
        {
            Destroy(this.gameObject);
        }
        else if(collider.tag == "Player")
        {
            Destroy(player.gameObject);
           // AudioBehaviour.instance.Play("nome effetto");
        }
    }
}
