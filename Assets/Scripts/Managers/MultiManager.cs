using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;

public class MultiManager : WebSocketIO
{
    public string playerPID;
    public string nowCityCID;

    public bool multiFlag;

    // Data Queue
    private Queue<string> dataQueue;

    public GameObject player;
    public int characterType;
    public GameObject[] playerPrefabList;

    // Old Position
    private Vector3 oldPosition;
    private Vector3 oldRotation;

    // Other Players
    private List<GameObject> playerList;
    private Dictionary<string, int> characterTypeList;

    public GameObject ProfileManagerInstance;
    public ProfileManager profileManager;

    void Start()
    {
        dataQueue = new Queue<string>();

        ProfileManagerInstance = GameObject.Find("ProfileManager");
        profileManager = ProfileManagerInstance.GetComponent<ProfileManager>();

        // Web Socket
        wsService = "/multi";

        Register();
        wsClient.OnMessage += (sender, e) => 
        {
            Debug.Log(((WebSocket)sender).Url + " : " + e.Data);
            dataQueue.Enqueue(e.Data);
        };

        playerList = new List<GameObject>();
        characterTypeList = new Dictionary<string, int>();

        multiFlag = false;
    }

    void Update()
    {
        if (multiFlag)
        {
            UploadPosition(); // Upload this player's position
            SetPosition(); // Update other player's position
        }
    }

    public void Sync()
    {
        playerPID = profileManager.playerPID;
        nowCityCID = profileManager.playerProfile.last_city;
        characterType = profileManager.playerProfile.model;
    }

    public void StartMulti()
    {
        Sync();
        
        player = Instantiate(playerPrefabList[characterType]);

        string[] last_pos = profileManager.playerProfile.last_pos.Split(":");
        player.transform.position = new Vector3(float.Parse(last_pos[0]), float.Parse(last_pos[1]), float.Parse(last_pos[2]));
        
        // player.GetComponent<PlayerController>().enabled = true;
        player.name = "PLAYER:" + profileManager.playerProfile.nickname;

        oldPosition = Vector3.zero;
        oldRotation = Vector3.zero;

        // Upload position by 1sec
        InvokeRepeating("UploadPosition", 0f, 1f);

        multiFlag = true;
    }

    public void StopMulti()
    {
        multiFlag = false;

        wsClient.Send(profileManager.playerProfile.nickname + ":GAME_EXIT");

        CancelInvoke("UploadPosition");

        Destroy(player);

        for (int idx = 0; idx < playerList.Count; idx++) 
        {
            Destroy(playerList[idx]);
            playerList.RemoveAt(idx);
        }
    }

    public void UploadPosition()
    {
        Vector3 pos = player.transform.position;
        Vector3 rot = player.transform.rotation.eulerAngles;

        if (oldPosition == pos && oldRotation == rot) 
        {
            return;
        }

        string query = player.name + ":" + nowCityCID + ":" + Round3(pos.x).ToString() + ":" + Round3(pos.y).ToString() + ":" + Round3(pos.z).ToString() + ":";
        query += rot.x.ToString() + ":" + rot.y.ToString() + ":" + rot.z.ToString() + ":" + characterType;
        wsClient.Send(query);

        oldPosition = pos;
        oldRotation = rot;
    }

    public void SetPosition()
    {
        while (dataQueue.Count > 0)
        {
            string[] data = dataQueue.Dequeue().Split(':');

            if (!data[1].Equals(nowCityCID))
            {
                continue;
            }

            // self
            if (data[0].Equals(player.name)) 
            {
                continue;
            }

            // player exited a game
            int idx = playerList.FindIndex(x => x.name.Equals(data[0]));
            if (data[1].Equals("GAME_EXIT")) 
            {
                Destroy(playerList[idx]);
                playerList.RemoveAt(idx);
                characterTypeList.Remove(data[0]);
                continue;
            }

            // New player joined
            if (idx == -1) 
            {
                GameObject newPlayer = Instantiate(playerPrefabList[int.Parse(data[8])]);
                newPlayer.name = data[0];
                newPlayer.transform.position = new Vector3(float.Parse(data[2]), float.Parse(data[3]), float.Parse(data[4]));
                newPlayer.transform.rotation = Quaternion.Euler(float.Parse(data[5]), float.Parse(data[6]), float.Parse(data[7]));
                playerList.Add(newPlayer);
                characterTypeList.Add(data[0], int.Parse(data[8]));
            }
            // Update position
            else 
            {
                if (int.Parse(data[8]) != characterTypeList[data[0]]) 
                {
                    Destroy(playerList[idx]);
                    playerList.RemoveAt(idx);

                    GameObject newPlayer = Instantiate(playerPrefabList[int.Parse(data[8])]);
                    newPlayer.name = data[0];
                    newPlayer.transform.position = new Vector3(float.Parse(data[2]), float.Parse(data[3]), float.Parse(data[4]));
                    newPlayer.transform.rotation = Quaternion.Euler(float.Parse(data[5]), float.Parse(data[6]), float.Parse(data[7]));
                    playerList.Add(newPlayer);
                    characterTypeList[data[0]] = int.Parse(data[8]);
                }
                else 
                {
                    playerList[idx].transform.position = new Vector3(float.Parse(data[2]), float.Parse(data[3]), float.Parse(data[4]));
                    playerList[idx].transform.rotation = Quaternion.Euler(float.Parse(data[5]), float.Parse(data[6]), float.Parse(data[7]));
                }
            }
        }
    }

    public GameObject GetPlayerByName(string name)
    {
        int idx = playerList.FindIndex(x => x.name.Equals(name));
        if (idx == -1) 
        {
            return null;
        }
        else 
        {
            return playerList[idx];
        }
    }

    // Round3
    private float Round3(float x) 
    {
        return Mathf.RoundToInt(x * 100000f) / 100000f;
    }
}
