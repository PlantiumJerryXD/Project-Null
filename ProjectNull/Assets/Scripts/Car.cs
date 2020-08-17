using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float AccelertionStat = 25;

    
    public float MaxSpeed = 100;
    Rigidbody Body;
    bool Drift = false;

    public GameObject CarControllerManager;

    public Vector3 ForceForward;
    
    void Start()
    {
        Body = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Accelerate();
        Drift = Input.GetAxis("Drift") == 1;
        CapSpeed();
    }

    void CapSpeed()
    {
        float Speed = Body.velocity.magnitude;

        if (!Drift)
        {
            float y = Body.velocity.y;
            Body.velocity = ForceForward * (Speed * Vector3.Dot(ForceForward,Body.velocity.normalized));
            Body.velocity = new Vector3(Body.velocity.x, y, Body.velocity.z);
            //Debug.Log(Vector3.Dot(transform.forward, Body.velocity.normalized));
        } 
        if (Speed > MaxSpeed)
        {
            Body.velocity = Body.velocity.normalized * MaxSpeed;
        }
        
    }

    void Accelerate()
    {
        Body.AddForce(Time.deltaTime * AccelertionStat * ForceForward * Input.GetAxis("Vertical"),ForceMode.Acceleration);

        if (Input.GetAxis("Vertical") == 0)
        {
            Body.drag = 1;
        } else
        {
            Body.drag = .1f;
        }

    }
}
