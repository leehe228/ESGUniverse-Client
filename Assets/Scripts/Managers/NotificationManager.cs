using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class NotificationManager : HttpPostIO
{
    List<NotiVO> notiList;

    public string playerPID;

    public InputField NIDInputField;
    public InputField TitleInputField;
    public InputField ContentInputField;

    public Text ListText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadNotificationDetailButton()
    {
        string NID = NIDInputField.text;
        CallLoadNotificationDetail(NID);
    }

    public void UploadNotificationButton()
    {
        string NID = NIDInputField.text;
        string PID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        string title = TitleInputField.text;
        string content = ContentInputField.text;
        CallUploadNotification(NID, PID, 0, title, content);
    }

    public void DeleteNotificationButton()
    {
        string NID = NIDInputField.text;
        CallDeleteNotification(NID);
    }

    public void ReadNotificationButton()
    {
        string NID = NIDInputField.text;
        CallReadNotification(NID);
    }

    public void CallUploadNotification(string NID, string PID, int notiType, string title, string content)
    {
        NotiVO newNoti = new NotiVO 
        {
            nid = NID,
            pid = PID,
            noti_type = notiType,
            title = title,
            content = content,
            is_read = 0
        };

        SendJson("/upload_noti_detail", ToJsonString<NotiVO>(newNoti));
        // newNoti.Dispose();
    }

    public void CallLoadAllNotifications()
    {
        playerPID = GameObject.Find("ProfileManager").GetComponent<ProfileManager>().playerPID;
        SendJson("/load_noti_list", "{\"pid\":\"" + playerPID + "\"}");
    }

    public void CallDeleteAllNotifications()
    {
        if (notiList.Count == 0) return;

        for (int i = 0; i < notiList.Count; i++)
        {
            CallDeleteNotification(notiList[i].nid);
        }

        CallLoadAllNotifications();
    }

    public void CallDeleteReadNotifications()
    {
        if (notiList.Count == 0) return;

        for (int i = 0; i < notiList.Count; i++)
        {
            if (notiList[i].is_read == 1) 
            {
                CallDeleteNotification(notiList[i].nid);
            }
        }

        CallLoadAllNotifications();
    }

    public void CallReadAllNotifications()
    {
        if (notiList.Count == 0) return;

        for (int i = 0; i < notiList.Count; i++)
        {
            if (notiList[i].is_read == 0) 
            {
                CallReadNotification(notiList[i].nid);
            }
        }

        CallLoadAllNotifications();
    }

    public void CallDeleteNotification(string NID)
    {
        SendJson("/delete_noti", "{\"nid\":\"" + NID + "\"}");
    }
    
    public void CallReadNotification(string NID)
    {
        SendJson("/update_noti_isread", "{\"nid\":\"" + NID + "\"}");
    }   

    public void CallLoadNotificationDetail(string NID)
    {
        SendJson("/load_noti_detail", "{\"nid\":\"" + NID + "\"}");
    }

    public void UploadNotification(string data)
    {
        Debug.Log(data);
        CallLoadAllNotifications();
    }

    public void LoadAllNotifications(string data)
    {
        data = "{\"target\":" + data + "}";
        Debug.Log(data);
        notiList = JsonUtility.FromJson<Serialization<NotiVO>>(data).ToList();
        
        ListText.text = "Notifications (" + notiList.Count + ")\n";
        string notiContents = "";

        for (int i = 0; i < notiList.Count; i++)
        {
            string content = "nid : " + notiList[i].nid + " / type : " + notiList[i].noti_type + "(" + notiList[i].is_read + ")\n";
            content += notiList[i].title + "\n" + notiList[i].content + "\n" + notiList[i].update_time;
            content += "\n------------------\n";
            notiContents += content;
        }

        ListText.text = notiContents;
    }

    public void DeleteNotification(string data)
    {
        Debug.Log(data);
        CallLoadAllNotifications();
    }
    
    public void ReadNotification(string data)
    {
        Debug.Log(data);
        CallLoadAllNotifications();
    }

    public void LoadNotificationDetail(string data)
    {
        Debug.Log(data);
    }

    public override void PassResult(string service, string data) 
    {
        if (service.Equals("/load_noti_list"))
        {
            LoadAllNotifications(data);
        }

        else if (service.Equals("load_noti_detail"))
        {
            LoadNotificationDetail(data);
        }

        else if (service.Equals("/upload_noti_detail"))
        {
            UploadNotification(data);
        }

        else if (service.Equals("/delete_noti"))
        {
            DeleteNotification(data);
        }

        else if (service.Equals("/update_noti_isread"))
        {
            ReadNotification(data);
        }


    }

}
