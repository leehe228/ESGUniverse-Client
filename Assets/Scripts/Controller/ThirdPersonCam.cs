using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform follow;
    [SerializeField] float m_Speed;
    Vector2 m_Input;

    public GameObject player;
    public bool timeonFlag, adjustFlag;

    void Start()
    {
        timeonFlag = false;
        adjustFlag = false;
    }

    void Rotate()
    {
        if (m_Input.magnitude != 0)
        {
            Quaternion q = follow.rotation;
            float xvalue;

            if (q.eulerAngles.x + m_Input.y * m_Speed > 20f)
            {
                xvalue = 20f;
            }
            else if (q.eulerAngles.x + m_Input.y * m_Speed < 1f)
            {
                xvalue = 1f;
            }
            else 
            {
                xvalue = q.eulerAngles.x + m_Input.y * m_Speed;
            }

            q.eulerAngles = new Vector3(xvalue, q.eulerAngles.y + m_Input.x * m_Speed, q.eulerAngles.z);
            follow.rotation = q;
        }
    }

    public void AdjustCamera()
    {
        // float xrot = follow.rotation.x;
        // float zrot = follow.rotation.z;
        // follow.transform.rotation = Quaternion.Lerp(follow.transform.rotation, Quaternion.Euler(xrot, player.transform.rotation.y, zrot), 3f * Time.deltaTime);
        follow.transform.rotation = Quaternion.Lerp(follow.transform.rotation, player.transform.rotation, 2f * Time.deltaTime);
    }

    public void AdjustCameraOn()
    {
        adjustFlag = true;
    }

    public void Update()
    {
        m_Input.x = Input.GetAxis("Mouse X");
        m_Input.y = -Input.GetAxis("Mouse Y");

        Rotate();

        if (adjustFlag)
        {
            AdjustCamera();
        }

        if (!timeonFlag)
        {
            Debug.Log(Mathf.Abs(player.transform.rotation.y - follow.transform.rotation.y));
            if (Mathf.Abs(player.transform.rotation.y - follow.transform.rotation.y) > 0.03f)
            {
                Debug.Log("Invoked");
                timeonFlag = true;
                Invoke("AdjustCameraOn", 2f);
            }
        }
        
        if (timeonFlag && m_Input.magnitude > 0.2f)
        {
            Debug.Log("InvokeCanceled");
            timeonFlag = false;
            adjustFlag = false;
            CancelInvoke("AdjustCameraOn");
        }

        Vector3 lerppos = Vector3.Lerp(follow.position, player.transform.position, 1.5f * Time.deltaTime);
        follow.position = lerppos;
    }
}