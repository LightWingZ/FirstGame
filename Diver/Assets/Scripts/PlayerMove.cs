using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    float rotateSpeed = 2.0f;

    float strafeSpeed = 10.0f;
    float forwardSpeed = 10.0f;
    float backSpeed = 10.0f;

    float lookLeftRight = 0.0f;
    float lookUpDown = 0.0f;

	// Use this for initialization
	void Start () {
        backSpeed = forwardSpeed * 0.7f;
    }
	
	// Update is called once per frame
	void Update () {
        
        // rotate the user's prospective
        lookLeftRight += rotateSpeed * Input.GetAxis("Mouse X");
        lookUpDown -= rotateSpeed * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(lookUpDown, lookLeftRight, 0.0f);


        // move the user
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);
        bool moved = false;

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.W))
        {
            move = new Vector3(transform.forward.normalized.x* forwardSpeed * Time.deltaTime, 0.0f, transform.forward.normalized.z * forwardSpeed * Time.deltaTime);
            moved = true;
        } else if (Input.GetKey(KeyCode.S))
        {
            move = new Vector3(transform.forward.normalized.x * forwardSpeed * Time.deltaTime*-1.0f, 0.0f, transform.forward.normalized.z * forwardSpeed * Time.deltaTime*-1.0f);
            moved = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move = new Vector3(transform.right.normalized.x * strafeSpeed * Time.deltaTime*-1.0f, 0.0f, transform.right.normalized.z * strafeSpeed * Time.deltaTime*-1.0f);
            moved = true;
        } else if (Input.GetKey(KeyCode.D))
        {
            move = new Vector3(transform.right.normalized.x * strafeSpeed * Time.deltaTime, 0.0f, transform.right.normalized.z * strafeSpeed * Time.deltaTime);
            moved = true;
        }
        if (moved)
        {
            transform.Translate(move, Space.World);
        }

    }
}
