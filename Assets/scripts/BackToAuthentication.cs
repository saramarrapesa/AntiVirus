using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToAuthentication : MonoBehaviour
{
    public void BacktoProfile()
    {
        SceneManager.LoadScene("Authentication");
    }
}
