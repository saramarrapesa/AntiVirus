using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;


public class LogForPlayer : MonoBehaviour
{
    public Leaderboard leaderboard;
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
     /* mainPage.MainPageLoginBtn.onClick.AddListener(OpenLoginPage);
        mainPage.MainPageSignUpBtn.onClick.AddListener(OpenSignupPage);
        mainPage.MainPageGuestBtn.onClick.AddListener(StartGuestSession);

        signUpPage.SignUpBtn.onClick.AddListener(UserSignUp);
        loginPage.LoginBtn.onClick.AddListener(LoginThroughBtn);

        gamePage.SetNameBtn.onClick.AddListener(ResetPlayerName);
        gamePage.LogoutBtn.onClick.AddListener(LogoutThroughBtn);
        #endregion
     */
        #region CheckingGameSession
        // To prevent showing login page everytime when player start the game
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                OpenGamePage();
                Debug.Log("session is valid, you can start a game session");
            }
            else
            {
                mainPage.MainPage.SetActive(true);
                Debug.Log("session is NOT valid, we should show the login form");
            }
        });
        #endregion
    }

    #region Signup and Login

    public void UserSignUpRoutine()
    {
        StartCoroutine(SetupUserSignUpRoutine());
    }

    IEnumerator SetupUserSignUpRoutine()
    {
        yield return UserSignUp();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }
    IEnumerator UserSignUp()
    {
        bool done = false;
        string email = signUpPage.SignUpEmailInput.text;
        string password = signUpPage.SignUpPasswordInput.text;
        string nickname = signUpPage.UsernameInput.text;

        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while creating user");
                done = true;
                return;
                
            }
            Debug.Log("user created successfully");
            LootLockerSDKManager.WhiteLabelLogin(email, password, true, (response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error while logging in");
                    done = true;
                    return;
                }
                LootLockerSDKManager.StartWhiteLabelSession((response) =>
                {
                    if (!response.success)
                    {
                        Debug.Log("error starting LootLocker session");
                        done = true;
                        return;
                    }
                    done = true;
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    Debug.Log("session started successfully");
                    if (nickname == "")
                    {
                        nickname = response.public_uid;
                    }

                    SetPlayerName(nickname);
                });
            });
            OpenGamePage();
        });
        yield return new WaitWhile(() => done == false);
    }

    IEnumerator UserLogin(string email, string password, bool rememberMe)
    {
        bool done = false;
        LootLockerSDKManager.WhiteLabelLogin(email, password, rememberMe, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while logging in");
                done = true;
                return;
            }
            LootLockerSDKManager.StartWhiteLabelSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error starting LootLocker session");
                    done = true;
                    return;
                }
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                Debug.Log("session started successfully");
                OpenGamePage();
                done = true;
            });
        });
        yield return new WaitWhile(() => done == false);
    }

    public void LoginThroughBtn()
    {
        StartCoroutine(LoginCredentials());
    }
    IEnumerator LoginCredentials()
    {
        string email = loginPage.LoginEmailInput.text;
        string password = loginPage.LoginPasswordInput.text;
        bool remember = true;
        yield return UserLogin(email, password, remember);
    }
    #endregion
    /*
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
    */
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
    void SetPlayerName(string name)
    {
        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
            }
            else
            {
                Debug.Log("Error setting player name");
            }
        });
    }

    void ResetPlayerName()
    {
        SetPlayerName(gamePage.ChangeUsernameInput.text);
        GetPlayerName();
    }
    /*
    void SetPlayerNickName()
    {
        SetPlayerName(signUpPage.UsernameInput.text);
    }*/

    void GetPlayerName()
    {
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retrieved player name: " + response.name);
                gamePage.Username.text = "Player Name: " + response.name;

            }
            else
            {
                Debug.Log("Error getting player name");

            }
        });
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
        yield return new WaitForSeconds(.9f);
        mainPage.MainPage.SetActive(false);
        loginPage.LoginPage.SetActive(false);
        signUpPage.SignUpPage.SetActive(false);
        gamePage.GamePage.SetActive(true);
        GetPlayerName();
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
#endregion