using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthPanel : MonoBehaviour
{
    public GameObject authPanel;
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject profilePanel;
    public void GetAuthPanelFromLoginPanel()
    {

        authPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void GetAuthPanelFromRegisterPanel()
    {

        authPanel.SetActive(true);
        registerPanel.SetActive(false);
    }


    public void GetLoginPanel()
    {
        authPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
    public void GetRegisterPanel()
    {
        authPanel.SetActive(false);
        registerPanel.SetActive(true);
    }
    public void GetProfilePanel()
    {
        authPanel.SetActive(false);
        profilePanel.SetActive(true);
    }
}
