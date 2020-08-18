using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float AccelertionStat = 25;

    float O;
    public float MaxSpeed = 100;
    Rigidbody Body;
    bool Drift = false;

    public Vector3 ForceForward;
    [SerializeField]
    bool IsPlayer = false;
    void Start()
    {
        O = Time.fixedDeltaTime;
        Body = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayer)
        {
            Accelerate();
            Drift = Input.GetAxis("Drift") == 1;
            CapSpeed();

            if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = .1f;
                Time.fixedDeltaTime = O * .1f;
            }
            else
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = O;
            }
        }
        
    }

    public void CapSpeed()
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

    public void Accelerate()
    {
        if (IsPlayer)
        {
            Body.AddForce(Time.deltaTime * AccelertionStat * ForceForward * Input.GetAxis("Vertical"), ForceMode.Acceleration);

            if (Input.GetAxis("Vertical") == 0)
            {
                Body.drag = 1;
            }
            else
            {
                Body.drag = .1f;
            }
        } else
        {
            Body.AddForce(Time.deltaTime * AccelertionStat * ForceForward, ForceMode.Acceleration);
        }

    }
}
