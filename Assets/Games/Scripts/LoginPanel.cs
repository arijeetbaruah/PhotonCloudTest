using Newtonsoft.Json;
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

            PlayFab.PlayFabClientAPI.ExecuteCloudScript(new PlayFab.ClientModels.ExecuteCloudScriptRequest
            {
                GeneratePlayStreamEvent = true,
                FunctionName = "AcceptCommonGroupUser",
                FunctionParameter = new Dictionary<string, object>
                {
                    { "GroupId", "5448014428B53D6F"},
                    { "currentPlayerId", "556CF7422082128A"},
                }
            }, result =>
            {
                Debug.Log(result.ToJson());
            }, ErrorCallback);


            //PlayFab.PlayFabGroupsAPI.AcceptGroupApplication(new PlayFab.GroupsModels.AcceptGroupApplicationRequest
            //{
            //    Entity = new PlayFab.GroupsModels.EntityKey
            //    {
            //        Id = "17BD503CA4CDB5FE",
            //        Type = "title_player_account"
            //    },
            //    Group = new PlayFab.GroupsModels.EntityKey
            //    {
            //        Id = "5448014428B53D6F",
            //        Type = "group"
            //    }
            //}, result =>
            //{
            //    Debug.Log(result.ToJson());
            //}, ErrorCallback);

            //PlayFab.PlayFabGroupsAPI.ApplyToGroup(new PlayFab.GroupsModels.ApplyToGroupRequest
            //{
            //    Entity = new PlayFab.GroupsModels.EntityKey
            //    {
            //        Id = "17BD503CA4CDB5FE",
            //        Type = "title_player_account"
            //    },
            //    Group = new PlayFab.GroupsModels.EntityKey
            //    {
            //        Id = "5448014428B53D6F",
            //        Type = "group"
            //    }
            //}, result =>
            //{
            //    Debug.Log(result.ToJson());
            //}, ErrorCallback);

        }, error =>
        {
            OpenRegister(false);
        });
    }

    public void CreateGroup()
    {
        PlayFab.PlayFabGroupsAPI.CreateGroup(new PlayFab.GroupsModels.CreateGroupRequest
        {
            GroupName = "Test Group"
        }, result1 =>
        {
            Debug.Log($"Group Created: {result1.GroupName}");
            PlayFab.PlayFabServerAPI.GetUserAccountInfo(new PlayFab.ServerModels.GetUserAccountInfoRequest
            {
                PlayFabId = "556CF7422082128A",
            }, r =>
            {
                PlayFab.GroupsModels.EntityKey entityKey = new PlayFab.GroupsModels.EntityKey
                {
                    Id = r.UserInfo.TitleInfo.TitlePlayerAccount.Id,
                    Type = r.UserInfo.TitleInfo.TitlePlayerAccount.Type
                };

                PlayFab.PlayFabGroupsAPI.InviteToGroup(new PlayFab.GroupsModels.InviteToGroupRequest
                {
                    Group = result1.Group,
                    Entity = entityKey
                }, result =>
                {
                    Debug.Log(result.ToJson());
                    PlayFab.PlayFabGroupsAPI.AcceptGroupApplication(new PlayFab.GroupsModels.AcceptGroupApplicationRequest
                    {
                        Entity = entityKey,
                        Group = result1.Group
                    }, r2 =>
                    {
                        Debug.Log(r2.ToJson());
                    }, ErrorCallback);

                }, ErrorCallback);
            }, null);
            //PlayFab.PlayFabClientAPI.ExecuteCloudScript(new PlayFab.ClientModels.ExecuteCloudScriptRequest
            //{
            //    FunctionName = "AddCommonMembersInTitleGroup",
            //    GeneratePlayStreamEvent = true,
            //    FunctionParameter = new Dictionary<string, object>
            //    {
            //        { "GroupId", result1.Group.Id },
            //        { "currentPlayerId", "556CF7422082128A" },
            //    }
            //}, result =>
            //{
            //    Debug.Log(result.ToJson());
            //}, ErrorCallback);
        }, ErrorCallback);
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
