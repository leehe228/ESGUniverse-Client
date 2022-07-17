using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;
using TMPro;
using UnityEngine.UI;

public class ChatManager : WebSocketIO
{
    // city name
    public string nowMapMID;
    public string playerPID;
    public string playerNickname;

    // Data Queue
    private Queue<string> chatQueue;

    private Camera mainCamera;

    public InputField playerChatInputField;

    void Start()
    {
        nowMapMID = "none";

        chatQueue = new Queue<string>();
        wsService = "/ws/chat/test/";

        Register();
        wsClient.OnMessage += (sender, e) => 
        {
            Debug.Log(((WebSocket)sender).Url + " : " + e.Data);
            chatQueue.Enqueue(e.Data);
        };

        // Camera
        mainCamera = Camera.main;

        // Chat Input Field
        playerChatInputField.onSubmit.AddListener(delegate
        {
            if (playerChatInputField.text.Length > 0) 
            {
                Sync();
		Send("{\"message\":\"" + playerChatInputField.text + "\"}");
                // Send(nowMapMID + ":" + playerNickname + ":" + playerChatInputField.text);
            }
        });

        StartChat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sync()
    {
        playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        nowMapMID = GameObject.Find("MapManager").GetComponent<MapManager>().nowMapMID;
        playerNickname = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerProfile.nickname;
    }

    public void StartChat() 
    {
        
    }

    public void StopChat() 
    {
        
    }

    public void PrintChat()
    {
        Sync();

        while(chatQueue.Count > 0) 
        {
            string[] data = chatQueue.Dequeue().Split(':');

            if (data[0].Equals(nowMapMID))
            {
                if (data[1].Equals(playerNickname)) 
                {
                    // 
                    Debug.Log("self : " + data);
                    continue;
                }
                

                GameObject target = GameObject.Find("MultiManager").GetComponent<MultiManager>().GetPlayerByName(data[1]);

                if (target) 
                {
                    Debug.Log(data);
                }
            }
        }
    }

    private Vector3 ToScreenPoint(Transform target) 
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(new Vector3(target.position.x + 0f, target.position.y + 2.5f, target.position.z + 0f));
        return new Vector3(screenPos.x, screenPos.y, transform.position.z);
    }
}
