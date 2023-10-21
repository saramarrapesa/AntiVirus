using LootLocker.Requests;
using UnityEngine;

public class LoginButton : MonoBehaviour
{
    public GameObject loginButton;
    public GameObject logoutButton;
    public GameObject loginPage;
    public GameObject profilePage;
    // Start is called before the first frame update
    void Start()
    {
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                logoutButton.SetActive(true);
                loginButton.SetActive(false);
                Debug.Log("session is valid, you can start a game session");

            }
            else
            {
                logoutButton.SetActive(false);
                loginButton.SetActive(true);
                Debug.Log("session is NOT valid, we should show the login form");

            }
        });
    }

    public void LoginButtonOpenLoginPanel()
    {
        profilePage.SetActive(false);
        loginPage.SetActive(true);
    }
}
