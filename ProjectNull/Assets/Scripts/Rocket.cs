using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    public GameObject prefab;
    public GameObject cone;
    public GameObject body;
    private MeshCollider coneCol;
    private CapsuleCollider bodyCol;
    public float ms = 10;
    float alive;
    // Start is called before the first frame update
    void Start()
    {
        prefab.transform.localScale = new Vector3(.5f, .5f, .5f);
        coneCol = cone.GetComponent<MeshCollider>();
        bodyCol = body.GetComponent<CapsuleCollider>();
        coneCol.enabled = false;
        bodyCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        prefab.transform.Translate(Vector3.forward * ms * Time.deltaTime);
        alive += Time.deltaTime;
        if (alive >= 2)
        {
            coneCol.enabled = true;
            bodyCol.enabled = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0,0,0);
        Destroy(prefab);
    }
}
