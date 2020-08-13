using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    Transform TargetPositionAndRotation;

    
    
    void Start()
    {
      


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = TargetPositionAndRotation.position;
        transform.rotation = TargetPositionAndRotation.rotation;
    }   
}
