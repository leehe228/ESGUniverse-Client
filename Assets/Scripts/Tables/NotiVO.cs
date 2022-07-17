using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class NotiVO 
{
    public string nid;
    public string pid;
    public int noti_type;
    public string title;
    public string content;
    public int is_read;
    public DateTime update_time;
}
