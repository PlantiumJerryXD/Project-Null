using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHoverAnim : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float UpDownMotion = .4f;
    
    float StartPos = 0;
    [SerializeField]
    GameObject Model;
    void Start()
    {
        StartPos = Model.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        Model.transform.localPosition = new Vector3(Model.transform.localPosition.x, StartPos + (UpDownMotion * Mathf.Sin(Time.time)), Model.transform.localPosition.z);
        
    }
}
