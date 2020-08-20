using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityUse : MonoBehaviour
{
    public GameObject Rocket;
    public GameObject Shield;
    public int rocketCD;
    public int shieldCD;
    float x,y,z;
    float shieldTime, rocketTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = (transform.position.x);
        y = (transform.position.y);
        z = (transform.position.z);
        if (Input.GetKeyDown(KeyCode.M)&&rocketTime >= rocketCD)
        {
            rocketTime = 0;
            Instantiate(Rocket, new Vector3(x, y, z), this.transform.rotation);
        }
        
        rocketTime += Time.deltaTime;
    }
}
