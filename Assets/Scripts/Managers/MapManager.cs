using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapManager : HttpPostIO
{
    public List<MapVO> mapList;
    public Text ListText;

    public string playerPID;
    public MapVO playerMap;
    public string playerMID;
    public MapVO nowMap;
    public string nowMapMID;

    // load map parameters
    int mapCount;
    int minRanking;
    int maxRanking;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Sync()
    {
        playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
    }

    public void SetParametersByCar()
    {
        mapCount = 2;
        minRanking = 6;
        maxRanking = 0;
    }

    public void LoadMapListButton()
    {
        SetParametersByCar();
        CallLoadMapList();
    }

    public void RefreshMapListButton()
    {
        CallLoadMapList();
    }

    public void CallLoadMap() 
    {
        Sync();
        SendJson("/load_map", "{\"pid\":\"" + playerPID + "\"}");
    }

    public void CallUploadMap()
    {
        Sync();
        SendJson("/upload_map", ToJsonString<MapVO>(playerMap));
    }

    public void CallLoadMapList()
    {
        SendJson("/list_up_city", "{\"count\":\"" + mapCount + "\",\"minRanking\":\"" + minRanking + "\",\"maxRanking\":\"" + maxRanking + "\"}");
    }

    public void LoadMap(string data)
    {
        Debug.Log(data);
        playerMap = JsonUtility.FromJson<MapVO>(data);
        playerMID = playerMap.mid;
    }

    public void UploadMap(string data)
    {
        Debug.Log(data);
    }

    public void LoadMapList(string data)
    {
        data = data.Replace("[", "").Replace("]", "").Replace("\"", "");
        Debug.Log(data);

        string[] mapMIDList = data.Split(",");

        ListText.text = "";
    }

    public override void PassResult(string service, string data) 
    {
        Debug.Log("Result Passed From " + service + ":" + data);

        if (service.Equals("/load_map"))
        {
            LoadMap(data);
        }

        else if (service.Equals("/upload_map"))
        {   
            UploadMap(data);
        }

        else if (service.Equals("/list_up_city"))
        {
            LoadMapList(data);
        }
    }
}
