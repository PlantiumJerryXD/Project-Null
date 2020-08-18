using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody Body;
    [SerializeField]
    TrackProgress Track;
    [SerializeField]
    Car Controller;



    Vector3 Target;
    void Start()
    {
        Body = transform.GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        Controller.Accelerate();
        Controller.CapSpeed();
        Rotate();
        Body.angularVelocity = new Vector3();

        if (Track.GetProgress(transform.position) + 1 < Track.TrackMarkers.Length)
        {
            //Debug.Log(Track.GetProgress(transform.position) + 1);
            //Target = Track.transform.localToWorldMatrix.MultiplyPoint(Track.TrackMarkers[Track.GetProgress(transform.position) + 1]);
        }
        else
        {

            //Target = Track.transform.localToWorldMatrix.MultiplyPoint(Track.TrackMarkers[0]);
        }
        //Body.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, Target - transform.position, .9f * Time.deltaTime));
        //Body.rotation = Quaternion.Euler(new Vector3(0, Body.rotation.eulerAngles.y, 0));
        AvoidCliffs();
        AvoidWalls();
    }
    void Rotate()
    {
        Body.angularVelocity = new Vector3();
        RaycastHit Hit;
        if (Physics.Raycast(transform.position - (Vector3.up * .5f), Vector3.down, out Hit))
        {

            Vector3 AlignedForward = Vector3.Cross(transform.right, Hit.normal);
            Controller.ForceForward = AlignedForward;
        }

    }

    void AvoidCliffs()
    {
        Debug.DrawRay(transform.position + (transform.forward * 35) - transform.right * 16, -Vector3.up * 4);
        Debug.DrawRay(transform.position + (transform.forward * 35) + transform.right * 16, -Vector3.up * 4);

        if (!Physics.Raycast(transform.position + (transform.forward * 35) - transform.right * 16, -Vector3.up, 8))
        {
            transform.rotation *= Quaternion.Euler(0, 70 * Time.deltaTime , 0);
        }
        if (!Physics.Raycast(transform.position + (transform.forward * 35) + transform.right * 16, -Vector3.up, 8))
        {
            transform.rotation *= Quaternion.Euler(0, -70 * Time.deltaTime, 0);
        }
    }

    void AvoidWalls()
    {
        Vector3 RotateTo = transform.forward;
        
        for (int i = 0; i <= 120; i+=4)
        {
            Vector3 Ang = transform.localToWorldMatrix.MultiplyVector(new Vector3(Mathf.Sin(Mathf.Deg2Rad * i),0, Mathf.Cos(Mathf.Deg2Rad * i)));
            Vector3 Ang2 = transform.localToWorldMatrix.MultiplyVector(new Vector3(Mathf.Sin(Mathf.Deg2Rad * -i), 0, Mathf.Cos(Mathf.Deg2Rad * -i)));
           


            RaycastHit Hit;

            if (Physics.Raycast(transform.position, Ang, out Hit,Body.velocity.magnitude * Time.fixedDeltaTime * 10))
            {
                RotateTo = Vector3.Lerp(Ang2, RotateTo, 1 / Body.velocity.magnitude * Hit.distance);
                Debug.DrawRay(transform.position, Ang * Body.velocity.magnitude * Time.fixedDeltaTime * 10, new Color(255, 0, 0));
                
            }
            if (Physics.Raycast(transform.position, Ang2, out Hit, Body.velocity.magnitude * Time.fixedDeltaTime * 10))
            {
                RotateTo = Vector3.Lerp(Ang, RotateTo, 1 / Body.velocity.magnitude * Hit.distance);
                Debug.DrawRay(transform.position, Ang2 * Body.velocity.magnitude * Time.fixedDeltaTime * 10, new Color(255,0,0));
            }

            

        }

        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(RotateTo),1 * Time.deltaTime);
    }



}
