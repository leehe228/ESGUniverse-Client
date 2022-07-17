using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using UnityEngine.Networking;

using UnityEngine.UI;

public class ProfileManager : HttpPostIO
{
    public ProfileVO playerProfile;
    public string playerPID;

    // Login
    public InputField login_email_inputfield;
    public InputField login_password_inputfield;
    public Text login_warning_text;

    // Register
    public InputField register_email_inputfield;
    public InputField register_password_inputfield;
    public InputField register_password2_inputfield;
    public InputField register_nickname_inputfield;
    public InputField register_name_inputfield;
    public Dropdown register_timezone_dropdown;
    public Text register_warning_text;

    // Secession
    public Text secession_warning_text;

    // flag
    public bool otherProfileFlag;

    void Start()
    {
        otherProfileFlag = false;
    }

    void Update()
    {
        
    }

    public void LoginButton()
    {
        string email = login_email_inputfield.text;
        string password = login_password_inputfield.text;

        if (email.Equals("") || password.Equals(""))
        {
            login_warning_text.text = "please enter all";
        }
        else 
        {
            login_warning_text.text = "";
            CallLogin(email, password);
        }
    }

    public void RegisterButton()
    {   
        string email = register_email_inputfield.text;
        string password = register_password_inputfield.text;
        string password2 = register_password2_inputfield.text;
        string nickname = register_nickname_inputfield.text;
        string name = register_name_inputfield.text;
        
        string timezone = register_timezone_dropdown.GetComponent<Dropdown>().options[register_timezone_dropdown.value].text;

        if (email.Equals("") || password.Equals("") || password2.Equals("") || nickname.Equals("") || name.Equals(""))
        {
            register_warning_text.text = "Please enter all";
        }
        else if (!password.Equals(password2))
        {
            register_warning_text.text = "Check confirm password";
        }
        else 
        {
            register_warning_text.text = "";
            CallRegister(email, password, nickname, name, timezone);
        }
    }

    public void SecessionButton()
    {
        CallSecession();
    }

    public void CallLogin(string email, string password)
    {
        password = Collections.EncodeUTF8(Collections.AESEncrypt128(password));

        SendPost("/login", "?email=" + email + "&password=" + password);
    }

    public void CallRegister(string email, string password, string nickname, string name, string timezone) 
    {
        password = Collections.EncodeUTF8(Collections.AESEncrypt128(password));

        ProfileVO newProfile = new ProfileVO()
        {
            email = email,
            password = password,
            nickname = nickname,
            name = name,
            player_timezone = timezone
        };

        SendJson("/register", ToJsonString<ProfileVO>(playerProfile));

        playerProfile.email = email;
        playerProfile.password = password;
        playerProfile.nickname = nickname;
        playerProfile.name = name;
        playerProfile.player_timezone = timezone;

        // newProfile.Dispose();
    }

    public void CallSecession() 
    {
        SendJson("/withdraw_membership", "{\"pid\":\"" + playerPID + "\"}");
    }

    public void CallLoadProfile() 
    {
        SendJson("/load_profile", "{\"pid\":\"" + playerPID + "\"}");
    }

    public void CallUploadProfile() 
    {
        SendJson("/upload_profile", ToJsonString<ProfileVO>(playerProfile));
    }

    public void CallGetNicknameByPID(string PID)
    {
        otherProfileFlag = true;
        SendJson("/load_profile", "{\"pid\":\"" + PID + "\"}");
    }

    public void Login(string data)
    {
        if (data.Equals("LOGIN_FAIL_NO_EMAIL"))
        {
            Debug.Log(data);
            login_warning_text.text = "Not registered email";
        }
        else if (data.Equals("LOGIN_FAIL"))
        {
            Debug.Log(data);
            login_warning_text.text = "Wrong Password";
        }
        else if (data.Equals("LOGIN_FAIL_ETC"))
        {
            Debug.Log(data);
            login_warning_text.text = "Error..";
        }
        else 
        {
            playerPID = data.Split("/")[1].Split(":")[1];
            login_warning_text.text = "Logined : " + playerPID;

            CallLoadProfile();
            GameObject.Find("MapManager").GetComponent<MapManager>().CallLoadMap();
            GameObject.Find("CityManager").GetComponent<CityManager>().CallLoadCity();
        }
    }

    public void Register(string data)
    {
        if (data.Equals("SIGNUP_SUCCESS"))
        {
            Debug.Log(data);
            register_warning_text.text = "Signed Up";
        }
        else if (data.Equals("SIGNUP_FAIL_EMAIL"))
        {
            Debug.Log(data);
            register_warning_text.text = "Aleardy registered email";
        }
        else if (data.Equals("SIGNUP_FAIL_ETC"))
        {
            Debug.Log(data);
            register_warning_text.text = "Error...";
        }
    }

    public void LoadProfile(string data) 
    {
        if (!otherProfileFlag)
        {
            playerProfile = JsonUtility.FromJson<ProfileVO>(data);
        }
        else 
        {
            ProfileVO newProile = JsonUtility.FromJson<ProfileVO>(data);
            otherProfileFlag = false;
        }
    }
    
    public void UploadProfile(string data)
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

    public void Secession(string data)
    {
        if (data.Equals("1")) 
        {
            Debug.Log(data);
            secession_warning_text.text = "PID : " + playerPID + " Deleted";
        }
        else 
        {
            Debug.Log(data);
            secession_warning_text.text = "Secession Fail";
        }
    }

    public override void PassResult(string service, string data) 
    {
        Debug.Log("Result Passed From " + service + ":" + data);

        if (service.Equals("/login"))
        {
            Login(data);
        }

        else if (service.Equals("/register"))
        {
            Register(data);
        }

        else if (service.Equals("/load_profile"))
        {
            LoadProfile(data);
        }

        else if (service.Equals("upload_profile")) 
        {
            UploadProfile(data);
        }

        else if (service.Equals("/withdraw_membership"))
        {
            Secession(data);
        }
    }
}
