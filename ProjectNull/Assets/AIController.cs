﻿using System.Collections;
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

    [SerializeField]
    float FramesLookAheadWallAvoidance = 25;
    [SerializeField]
    float WallAvoidanceFOV = 80;
    [SerializeField]
    float CliffAvoidanceFOV = 80;
    [SerializeField]
    float CliffAvoidanceLookAhead = 25;
    [SerializeField]
    float RandomizeParams = 5;

    [SerializeField]
    bool debug;
    Vector3 Target;
    void Start()
    {
        Body = transform.GetComponent<Rigidbody>();
        FramesLookAheadWallAvoidance += Random.Range(-RandomizeParams, RandomizeParams);
        WallAvoidanceFOV += Random.Range(-RandomizeParams, RandomizeParams);
        CliffAvoidanceFOV   += Random.Range(-RandomizeParams, RandomizeParams);
        CliffAvoidanceLookAhead += Random.Range(-RandomizeParams, RandomizeParams);

    }

    // Update is called once per frame
    void Update()
    {
        Controller.Accelerate();
        Controller.CapSpeed();
        Rotate();
        Body.angularVelocity = new Vector3();

        int index = Track.GetProgress(transform.position) + 1;

        if (index > Track.TrackMarkers.Length - 1) { index = 0; }
        
        Target = Track.transform.localToWorldMatrix.MultiplyPoint(Track.TrackMarkers[index]);


        if (debug)
        {
            Debug.DrawRay(transform.position, (Target - transform.position).normalized , new Color(255, 255, 0));
        }

        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward,(Target - transform.position).normalized,1 * Time.deltaTime));
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));

        
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
        
        Vector3 RotateTo = transform.forward;

        for (int i = 0; i <= CliffAvoidanceFOV; i += 4)
        {
            Vector3 Ang = transform.localToWorldMatrix.MultiplyVector(new Vector3(Mathf.Sin(Mathf.Deg2Rad * i), 0, Mathf.Cos(Mathf.Deg2Rad * i)));
            Vector3 Ang2 = transform.localToWorldMatrix.MultiplyVector(new Vector3(Mathf.Sin(Mathf.Deg2Rad * -i), 0, Mathf.Cos(Mathf.Deg2Rad * -i)));


            if (!Physics.Raycast(transform.position + Ang * Body.velocity.magnitude * Time.fixedDeltaTime * CliffAvoidanceLookAhead, Vector3.down, 8))
            {
                RotateTo = Vector3.Lerp(Ang2, RotateTo, .2f);
                if (debug)
                {
                    Debug.DrawRay(transform.position + Ang * Body.velocity.magnitude * Time.fixedDeltaTime * CliffAvoidanceLookAhead, Vector3.down, new Color(255, 0, 0));
                }
            }
            else
            {
                if (debug)
                {
                    Debug.DrawRay(transform.position + Ang * Body.velocity.magnitude * Time.fixedDeltaTime * CliffAvoidanceLookAhead, Vector3.down, new Color(0, 255, 0));
                }
            }

            if (!Physics.Raycast(transform.position + Ang2 * Body.velocity.magnitude * Time.fixedDeltaTime * CliffAvoidanceLookAhead, Vector3.down, 8))
            {
                RotateTo = Vector3.Lerp(Ang, RotateTo, .2f);
                if (debug)
                {
                    Debug.DrawRay(transform.position + Ang2 * Body.velocity.magnitude * Time.fixedDeltaTime * CliffAvoidanceLookAhead, Vector3.down, new Color(255, 0, 0));
                }
            }else
            {
                if (debug)
                {
                    Debug.DrawRay(transform.position + Ang2 * Body.velocity.magnitude * Time.fixedDeltaTime * CliffAvoidanceLookAhead, Vector3.down, new Color(0, 255, 0));
                }
            }
            
           
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RotateTo), Time.deltaTime * 2);
    }

    void AvoidWalls()
    {
        Vector3 RotateTo = transform.forward;
        float FrameLookAhead = FramesLookAheadWallAvoidance;
        float GreaterAnglesLessDistance = .5f;
        for (int i = 0; i <= WallAvoidanceFOV; i+=4)
        {
            Vector3 Ang = transform.localToWorldMatrix.MultiplyVector(new Vector3(Mathf.Sin(Mathf.Deg2Rad * i),0, Mathf.Cos(Mathf.Deg2Rad * i)));
            Vector3 Ang2 = transform.localToWorldMatrix.MultiplyVector(new Vector3(Mathf.Sin(Mathf.Deg2Rad * -i), 0, Mathf.Cos(Mathf.Deg2Rad * -i)));

            float DistanceModifier = 1; //Mathf.Lerp(1, GreaterAnglesLessDistance, (1 / 80) * i);

            LayerMask L = ~(1 << 2);
            RaycastHit Hit;

            if (Physics.Raycast(transform.position, Ang, out Hit,Body.velocity.magnitude * Time.fixedDeltaTime * FrameLookAhead * DistanceModifier,L))
            {
                RotateTo = Vector3.Lerp(Ang2, RotateTo, 1 / Body.velocity.magnitude * Hit.distance);
                if (debug)
                {
                    Debug.DrawRay(transform.position, Ang * Body.velocity.magnitude * Time.fixedDeltaTime * FrameLookAhead * DistanceModifier, new Color(255, 0, 0));
                }
                
            } else
            {
                if (debug)
                {
                    Debug.DrawRay(transform.position, Ang * Body.velocity.magnitude * Time.fixedDeltaTime * FrameLookAhead * DistanceModifier, new Color(255, 255, 255));
                }
            }
            if (Physics.Raycast(transform.position, Ang2, out Hit, Body.velocity.magnitude * Time.fixedDeltaTime * FrameLookAhead * DistanceModifier,L))
            {
                RotateTo = Vector3.Lerp(Ang, RotateTo, 1 / Body.velocity.magnitude * Hit.distance);
                if (debug)
                {
                    Debug.DrawRay(transform.position, Ang2 * Body.velocity.magnitude * Time.fixedDeltaTime * FrameLookAhead * DistanceModifier, new Color(255, 0, 0));
                }

            } else
            {
                if (debug)
                {
                    Debug.DrawRay(transform.position, Ang2 * Body.velocity.magnitude * Time.fixedDeltaTime * FrameLookAhead * DistanceModifier, new Color(255, 255, 255));
                }
            }

            

        }

        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(RotateTo),Time.deltaTime * 1);
    }



}