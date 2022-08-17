using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;
using System;

public static class PlayFabAuthSystem
{
    public const string AuthIDKey = "AuthID";

    public static string AuthID
    {
        get
        {
            return PlayerPrefs.GetString(AuthIDKey);
        }
        set
        {
            PlayerPrefs.SetString(AuthIDKey, value);
        }
    }

    public static void LoginWithEmailAndPassword(string email, string password, Action<LoginResult> OnSuccess, Action<PlayFabError> OnError)
    {
        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        }, OnSuccess, OnError);
    }

    public static void LinkWithCustomID(Action<LinkCustomIDResult> OnSuccess, Action<PlayFabError> OnError)
    {
        PlayFabClientAPI.LinkCustomID(new LinkCustomIDRequest
        {
            CustomId = AuthID
        }, OnSuccess, OnError);
    }

    public static void LoginWithCustomID(bool createAccount = false, Action<LoginResult> OnSuccess = null, Action<PlayFabError> OnError = null)
    {
        if (PlayerPrefs.HasKey(AuthIDKey))
        {
            string authID = AuthID;
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
            {
                CreateAccount = createAccount,
                CustomId = authID
            }, OnSuccess, OnError);
        }
        else
        {
            OnError?.Invoke(new PlayFabError
            {
                Error = PlayFabErrorCode.InvalidAuthToken,
                ErrorMessage = "Token Not Found"
            });
        }
    }

    public static void RegisterWithEmailAndPassword(string email, string displayName, string password, Action<RegisterPlayFabUserResult> OnSuccess, Action<PlayFabError> OnError)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            DisplayName = displayName,
            Username = displayName,
            Password = password,
            Email = email,
        }, OnSuccess, OnError);
    }
}
