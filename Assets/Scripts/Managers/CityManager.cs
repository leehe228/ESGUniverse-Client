using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using UnityEngine.Networking;

public class CityManager : HttpPostIO
{
    public CityVO playerCity;
    public string playerCID;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CallLoadCity()
    {
        string playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        SendJson("/load_city", "{\"pid\":\"" + playerPID + "\"}");
    }

    public void CallUploadCity()
    {
        SendJson("/upload_city", ToJsonString<CityVO>(playerCity));
    }

    public void LoadCity(string data)
    {
        playerCity = JsonUtility.FromJson<CityVO>(data);
        playerCID = playerCity.cid;
    }

    public void UploadCity(string data)
    {
        if (data.Equals("1")) 
            {
                Debug.Log(data);
            }
            else 
            {
                Debug.Log(data);
            }
    }

    public override void PassResult(string service, string data) 
    {
        Debug.Log("Result Passed From " + service + ":" + data);

        if (service.Equals("/load_city"))
        {
            LoadCity(data);
        }

        else if (service.Equals("/upload_city"))
        {
            UploadCity(data);
        }
    }
}
