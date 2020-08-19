using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody Body;
    float NormalTransitionTimer;
    Vector3 NormalTransition;
    Vector3 PrevForward;
    float Yaw = 0;


    [SerializeField]
    Car CarController;
    void Start()
    {
        Body = transform.GetComponent<Rigidbody>();
        Yaw = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        //CarController = transform.GetComponent<Car>();
    }

    void Rotate()
    {
        Body.angularVelocity = new Vector3();
        RaycastHit Hit;
        if (Physics.Raycast(transform.position -( Vector3.up * .5f), Vector3.down , out Hit)) 
        {
           
            Vector3 AlignedForward = Vector3.Cross(transform.right, Hit.normal);
            PrevForward = transform.forward;

            if (NormalTransitionTimer == 1)
            {
                CarController.ForceForward = AlignedForward;
                NormalTransition = AlignedForward;
                NormalTransitionTimer = 0;
            }
            Debug.DrawRay(transform.position, Hit.normal);
        }

        if (NormalTransitionTimer <= 1)
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.Lerp(PrevForward, NormalTransition, NormalTransitionTimer));
            NormalTransitionTimer += Time.deltaTime * 16 * (Mathf.Clamp(1 /(CarController.MaxSpeed) * Body.velocity.magnitude * Vector3.Dot(CarController.ForceForward,Body.velocity),.2f,1));
            NormalTransitionTimer = Mathf.Clamp(NormalTransitionTimer, 0, 1);
        }

        

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,Yaw, transform.rotation.eulerAngles.z);

        Yaw += Input.GetAxis("Horizontal") * Time.deltaTime * 65;
    }

    
    
}
