using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;

public class WebSocketIO : MonoBehaviour
{
    // WebSocket Instance
    protected WebSocket wsClient;
    protected static string wsAddress = "ws://158.247.235.218:8080";
    protected string wsService;

    protected void Register()
    {
        wsClient = new WebSocket(wsAddress + wsService);
        wsClient.Connect();
	Debug.Log("Registered!");
    }

    protected void Send(string data) 
    {   
        wsClient.Send(data);
    }
}
