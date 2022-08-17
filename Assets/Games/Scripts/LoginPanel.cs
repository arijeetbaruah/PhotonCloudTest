using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField l_email;
    [SerializeField] private TMP_InputField l_password;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField r_email;
    [SerializeField] private TMP_InputField r_password;
    [SerializeField] private TMP_InputField r_display;
    [SerializeField] private GameObject registerPanel;

    private void Start()
    {
        PlayFabAuthSystem.LoginWithCustomID(false, result =>
        {
            Debug.Log($"Login User: {result.PlayFabId}");
        }, error =>
        {
            OpenRegister(false);
        });
    }

    public void LoginWithEmail()
    {
        PlayFabAuthSystem.LoginWithEmailAndPassword(l_email.text, l_password.text, result =>
        {
            Debug.Log($"Login User: {result.PlayFabId}");
            PlayFabAuthSystem.AuthID = System.Guid.NewGuid().ToString();
            PlayFabAuthSystem.LinkWithCustomID(null, null);
        }, ErrorCallback);
    }

    public void Register()
    {
        PlayFabAuthSystem.RegisterWithEmailAndPassword(r_email.text, r_display.text, r_password.text, result =>
        {
            Debug.Log($"Registered User: {result.PlayFabId}");
            PlayFabAuthSystem.AuthID = System.Guid.NewGuid().ToString();
            PlayFabAuthSystem.LinkWithCustomID(null, null);
        }, ErrorCallback);
    }

    public void OpenRegister(bool isActive)
    {
        loginPanel.SetActive(!isActive);
        registerPanel.SetActive(isActive);
    }

    private void ErrorCallback(PlayFab.PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
