using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using System;

public class HttpPostIO : MonoBehaviour
{
    protected static string httpAddress = "http://api.esguniverse.kro.kr:5555";

    void Start() {}

    void Update() {}

    public void SendPost(string service, string query)
    {
        StartCoroutine(HttpUpload(service, query, "", 1));
    }

    public void SendJson(string service, string json) 
    {
        StartCoroutine(HttpUpload(service, "", json, 0));
    }

    IEnumerator HttpUpload(string service, string query, string json, int sendtype)
    {
        // JSON
        if (sendtype == 0) 
        {
            using (UnityWebRequest request = UnityWebRequest.Post(httpAddress + service, json))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(request.error);
                    PassResult(service, request.error);
                }
                else
                {
                    Debug.Log(request.downloadHandler.text);
                    PassResult(service, request.downloadHandler.text);
                }
            }
        }

        // POST
        else if (sendtype == 1) 
        {
            // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

            using (UnityWebRequest www = UnityWebRequest.Post(httpAddress + service + query, ""))
            {
                yield return www.SendWebRequest();

                if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError)) {
                    Debug.LogError(www.error);
                    PassResult(service, www.error);
                }
                else {
                    Debug.Log(www.downloadHandler.text);
                    PassResult(service, www.downloadHandler.text);
                }
            }
        }
    }

    public string ToJsonString<T>(T instance)
    {
        return JsonUtility.ToJson(instance);
    }

    public virtual void PassResult(string service, string data) 
    {
        
    }
}