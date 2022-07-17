using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ProfileVO
{
    public string pid;
    public string email;
    public string password;
    public string nickname;
    public string name;
    public int model;
    public string player_timezone;
    public DateTime player_last_uptime;
    public int main_mission;
    public string sub_mission;
    public string last_pos;
    public string last_city;
    public int balance;
    public int badge_state;
    public int license_state;
    public DateTime update_time;
    public string inventory;
    public string import_info;
    public string export_info;
    public string energy_import;
    public int vehicle_unlock;
    public string fuel_info;
}
