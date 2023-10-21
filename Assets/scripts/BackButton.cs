using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject SignUpPanel;
    public GameObject AuthPanel;
    public GameObject ProfilePanel;
    // Start is called before the first frame update
    public void BackFromLoginPage()
    {
        loginPanel.SetActive(false);
        AuthPanel.SetActive(true);
    }
    public void BackFromSignUpPage()
    {
        SignUpPanel.SetActive(false);
        AuthPanel.SetActive(true);
    }
    public void BackFromProfilePage()
    {
        ProfilePanel.SetActive(false);
        AuthPanel.SetActive(true);
    }
}
