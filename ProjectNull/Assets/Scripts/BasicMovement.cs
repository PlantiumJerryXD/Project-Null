using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= Vector3.right;
        }

        moveDirection.Normalize();

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}