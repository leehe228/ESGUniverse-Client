using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using UnityEngine.Networking;

using UnityEngine.UI;

public class FriendManager : HttpPostIO
{
    List<FriendVO> friendList;

    public string playerPID;
    public InputField friendPIDInputField;

    public Text ListText;


    public void Start()
    {
        
    }

    public void Update()
    {

    }

    public void RequestFriendButton()
    {
        string fpid = friendPIDInputField.text;
        CallRequestFriend(fpid);
    }

    public void AcceptFriendButton()
    {
        string fpid = friendPIDInputField.text;
        CallAcceptFriend(fpid);
    }

    public void DeleteFriendButton()
    {
        string fpid = friendPIDInputField.text;
        CallDeleteFriend(fpid);
    }

    public void PrintFriendListButton()
    {
        CallPrintFriendList();
    }

    public void CallRequestFriend(string friendPID)
    {
        playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        string message = "{\"pid\":\"" + friendPID + "\",\"noti_type\":2,\"title\":\"" + playerPID + "\",\"content\":\"\"}";
        SendJson("/request_friend", message);
        Debug.Log(message);
    }

    public void CallAcceptFriend(string friendPID)
    {
        playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        string message = "{\"pid1\":\"" + playerPID + "\",\"pid2\":\"" + friendPID + "\"}";
        SendJson("/accept_friend", message);
        Debug.Log(message);
    }

    public void CallDeleteFriend(string friendPID)
    {
        playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        string message = "{\"pid1\":\"" + playerPID + "\",\"pid2\":\"" + friendPID + "\"}";
        SendJson("/delete_friend", message);
        Debug.Log(message);
    }

    public void CallPrintFriendList()
    {
        playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        string message = "{\"pid1\":\"" + playerPID + "\"}";
        SendJson("/load_friend_list", message);
        Debug.Log(message);
    }

    public void RequestFriend(string data)
    {
        Debug.Log(data);
    }

    public void AcceptFriend(string data)
    {   
        Debug.Log(data);
        CallPrintFriendList();
    }

    public void DeleteFriend(string data)
    {
        Debug.Log(data);
        CallPrintFriendList();
    }

    public void LoadFriendList(string data)
    {
        data = "{\"target\":" + data + "}";
        friendList = JsonUtility.FromJson<Serialization<FriendVO>>(data).ToList();

        ListText.text = "Friends (" + friendList.Count + ")\n";
        string friendContent = "";

        for (int i = 0; i < friendList.Count; i++)
        {
            string content = "pid : " + friendList[i].pid2;
            content += "\n------------------\n";
            friendContent += content;
        }

        ListText.text = friendContent;
    }

    public override void PassResult(string service, string data) 
    {
        if (service.Equals("/request_friend"))
        {
            RequestFriend(data);
        }

        else if (service.Equals("/accept_friend"))
        {
            AcceptFriend(data);
        }

        else if (service.Equals("/delete_friend"))
        {
            DeleteFriend(data);
        }

        else if (service.Equals("/load_friend_list"))
        {
            LoadFriendList(data);
        }
    }
}
