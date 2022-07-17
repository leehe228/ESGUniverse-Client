using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Animator anim;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ControllPlayer();
    }

    public void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        float rot = gameObject.transform.rotation.eulerAngles.y;

        // Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movement = new Vector3(0f, 0.0f, moveVertical);
        Vector3 rotation = new Vector3(0f, moveHorizontal * 50f, 0f);

        gameObject.transform.Rotate(rotation * Time.deltaTime);

        Vector3 rotmove = Quaternion.AngleAxis(rot, Vector3.up) * movement;
        transform.Translate(rotmove * movementSpeed * Time.deltaTime, Space.World);
    }
}
