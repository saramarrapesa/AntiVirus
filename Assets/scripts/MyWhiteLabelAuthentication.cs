using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;


public class MyWhiteLabelAuthentication : MonoBehaviour
{

    #region Pages
    public MainPageStuffs mainPage;
    public LoginPageStuffs loginPage;
    public SignUpStuffs signUpPage;
    public GamePageStuffs gamePage;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region BtnInit
        mainPage.MainPageLoginBtn.onClick.AddListener(OpenLoginPage);
        mainPage.MainPageSignUpBtn.onClick.AddListener(OpenSignupPage);
        mainPage.MainPageGuestBtn.onClick.AddListener(StartGuestSession);

        signUpPage.SignUpBtn.onClick.AddListener(UserSignUp);
        loginPage.LoginBtn.onClick.AddListener(LoginThroughBtn);

        gamePage.SetNameBtn.onClick.AddListener(ResetPlayerName);
        gamePage.LogoutBtn.onClick.AddListener(LogoutThroughBtn);
        #endregion

        StartCoroutine(CheckGameSession());
    }

    #region CheckingGameSession
    IEnumerator CheckGameSession()
    {
        bool done = false;
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                Debug.Log("session is valid, you can start a game session");
                LootLockerSDKManager.StartWhiteLabelSession((response) =>
                {
                    if (!response.success)
                    {
                        Debug.Log("error starting LootLocker session");
                        return;
                    }
                    Debug.Log("session started successfully");
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    OpenGamePage();
                    done = true;
                }); 
            }
            else
            {
                mainPage.MainPage.SetActive(true);
                Debug.Log("session is NOT valid, we should show the login form");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    #endregion

    #region Signup and Login
    void UserSignUp()
    {
        string email = signUpPage.SignUpEmailInput.text;
        string password = signUpPage.SignUpPasswordInput.text;
        string nickname = signUpPage.UsernameInput.text;

        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while creating user");
                return;
            }
            Debug.Log("user created successfully");
            LootLockerSDKManager.WhiteLabelLogin(email, password, true, (response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error while logging in");
                    return;
                }
                LootLockerSDKManager.StartWhiteLabelSession((response) =>
                {
                    if (!response.success)
                    {
                        Debug.Log("error starting LootLocker session");
                        return;
                    }
                    Debug.Log("session started successfully");
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    StartCoroutine(SetAndGetPlayerName(nickname));
                    OpenGamePage();
                });
            });
        });
    }

    void UserLogin(string email, string password, bool rememberMe)
    {
        LootLockerSDKManager.WhiteLabelLogin(email, password, rememberMe, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while logging in");
                return;
            }
            LootLockerSDKManager.StartWhiteLabelSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error starting LootLocker session");
                    return;
                }
                Debug.Log("session started successfully");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                OpenGamePage();
            });
        });
    }

    void LoginThroughBtn()
    {
        string email = loginPage.LoginEmailInput.text;
        string password = loginPage.LoginPasswordInput.text;
        bool remember = true;
        UserLogin(email, password, remember);
    }
    #endregion

    #region GuestPlayer
    void StartGuestSession()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            StartGamePage();
            Debug.Log("successfully started LootLocker session");
        });
    }
    #endregion

    #region LogoutSession
    void Logout()
    {
        LootLockerSDKManager.EndSession(response =>
        {
            if (!response.success)
            {
                Debug.Log("error whith the log out");
            }
            else
            {
                Debug.Log("You are Logged out");
                OpenAuthPage();
            }
        });
    }

    void LogoutThroughBtn()
    {
        Logout();
    }

    #endregion

    #region PlayerName

    IEnumerator SetAndGetPlayerName(string name)
    {
        bool done = false;
        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
                LootLockerSDKManager.GetPlayerName((response) =>
                {
                    string username = response.name;
                    if (response.success)
                    {
                        Debug.Log("Successfully retrieved player name: " +username);
                        gamePage.Username.text = username;
                        done = true;

                    }
                    else
                    {
                        Debug.Log("Error getting player name");
                        done = true;

                    }
                });
                done = true;
            }
            else
            {
                Debug.Log("Error setting player name");
                done = true;
            }
        });
        
        yield return new WaitWhile(() => done == false);

    }

    void ResetPlayerName()
    {
        StartCoroutine(SetAndGetPlayerName(gamePage.ChangeUsernameInput.text));
    }

    public IEnumerator GetPlayerName()
    {
        bool done = false;
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            string username = response.name;
            if (response.success)
            {
                Debug.Log("Successfully retrieved player name: " + username);
                gamePage.Username.text = username;
                done = true;

            }
            else
            {
                Debug.Log("Error getting player name");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    #endregion

    #region PageOpenerFuntions

    #region OpenLoginPage
    void OpenLoginPage()
    {
        StartCoroutine(OnOpenLoginPage());
    }
    IEnumerator OnOpenLoginPage()
    {
        yield return new WaitForSeconds(.5f);
        mainPage.MainPage.SetActive(false);
        signUpPage.SignUpPage.SetActive(false);
        loginPage.LoginPage.SetActive(true);
    }
    #endregion

    #region OpenGamePage
    void OpenGamePage()
    {
        StartCoroutine(OnOpenGamePage());
    }
    IEnumerator OnOpenGamePage()
    {
        yield return new WaitForSeconds(.5f);
        mainPage.MainPage.SetActive(false);
        loginPage.LoginPage.SetActive(false);
        signUpPage.SignUpPage.SetActive(false);
        gamePage.GamePage.SetActive(true);
        StartCoroutine(GetPlayerName());
    }
    #endregion

    #region StartPlayPage
    void StartGamePage()
    {
        StartCoroutine(OnStartGamePage());
    }
    IEnumerator OnStartGamePage()
    {
        yield return new WaitForSeconds(.9f);
        mainPage.MainPage.SetActive(false);
        loginPage.LoginPage.SetActive(false);
        signUpPage.SignUpPage.SetActive(false);
        gamePage.GamePage.SetActive(false);
        SceneManager.LoadScene("SampleScene");

    }
    #endregion

    #region OpenAuthPage

    void OpenAuthPage()
    {
        StartCoroutine(OnOpenAuthPage());
    }

    IEnumerator OnOpenAuthPage()
    {
        yield return new WaitForSeconds(.9f);
        mainPage.MainPage.SetActive(true);
        loginPage.LoginPage.SetActive(false);
        signUpPage.SignUpPage.SetActive(false);
        gamePage.GamePage.SetActive(false);
    }
    #endregion

    #region OpenSignUpPage
    void OpenSignupPage()
    {
        StartCoroutine(OpOpenSignupPage());
    }
    IEnumerator OpOpenSignupPage()
    {
        yield return new WaitForSeconds(.5f);
        mainPage.MainPage.SetActive(false);
        signUpPage.SignUpPage.SetActive(true);
    }
    #endregion

    #endregion

   
}

#region Pages Class
[Serializable]
public class MainPageStuffs
{
    public GameObject MainPage;
    public Button MainPageLoginBtn;
    public Button MainPageSignUpBtn;
    public Button MainPageGuestBtn;

}
[Serializable]
public class LoginPageStuffs
{
    public GameObject LoginPage;
    public TMP_InputField LoginEmailInput;
    public TMP_InputField LoginPasswordInput;
    public Button LoginBtn;
   
}
[Serializable]
public class SignUpStuffs
{
    public GameObject SignUpPage;
    public TMP_InputField SignUpEmailInput;
    public TMP_InputField SignUpPasswordInput;
    public TMP_InputField UsernameInput;
    public Button SignUpBtn;

}

[Serializable]
public class GamePageStuffs
{
    public GameObject GamePage;
    public TMP_Text Username;
    public TMP_InputField ChangeUsernameInput;
    public Button SetNameBtn;
   // public Button GetNameBtn;
    public Button LogoutBtn;
    
}
#endregion

