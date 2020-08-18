using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackProgress : MonoBehaviour
{
    // Start is called before the first frame update

    
    public Vector3[] TrackMarkers;
    [SerializeField]
    float Progress;
    [SerializeField]
    float Length = 0;

    [SerializeField]
    Transform Kart;

    [SerializeField]
    bool DeriveFromChildren;

    float Lap = 0;
    void Start()
    {
        List<Vector3> Temp = new List<Vector3>();
        if (DeriveFromChildren)
        {
            foreach (Transform T in transform.GetComponentsInChildren<Transform>())
            {
                if (T != transform)
                {
                    Temp.Add(transform.worldToLocalMatrix.MultiplyPoint(T.position));
                    Destroy(T.gameObject);
                }
            }
            TrackMarkers = Temp.ToArray();
        }

        
        Length = 0;
        int i = 0;
        foreach (Vector3 M in TrackMarkers)
        {
            Length += Vector3.Distance(transform.localToWorldMatrix.MultiplyPoint(M), transform.localToWorldMatrix.MultiplyPoint(TrackMarkers[Mathf.RoundToInt(Mathf.PingPong(i + 1, TrackMarkers.Length - 1))]));
            i++;
        }
    }

    private void Update()
    {
        //GetProgress(Kart.position);
        

    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 M in TrackMarkers)
        {

            Gizmos.color = (new Color(255, 0, 0));
            Gizmos.DrawSphere(transform.localToWorldMatrix.MultiplyPoint(M), 5);
        }
    }

    public int GetProgress(Vector3 V)
    {
        Vector3 Closest = new Vector3(1,1,1) * Mathf.Infinity;
        int i = 0;
        foreach (Vector3 M in TrackMarkers)
        {
           
            if (Vector3.Distance(V, transform.localToWorldMatrix.MultiplyPoint(M)) < Vector3.Distance(V, Closest))
            {
                if (Vector3.Dot((transform.localToWorldMatrix.MultiplyVector(TrackMarkers[Mathf.RoundToInt(Mathf.PingPong(i + 1, TrackMarkers.Length - 1))]) - M).normalized, (M - transform.worldToLocalMatrix.MultiplyPoint(V)).normalized) < .5f)
                {

                    Closest = transform.localToWorldMatrix.MultiplyPoint(M);
                }
            }
            i++;
        }

        int Distance = 0;

        foreach (Vector3 M in TrackMarkers)
        {
            
            if (transform.localToWorldMatrix.MultiplyPoint(M) == Closest)
            {
                break;
            }
            Distance++;
        }

        //Lap = (1 / TrackMarkers.Length) * Distance;
        Progress = Distance;
        return Distance;


    }
}
