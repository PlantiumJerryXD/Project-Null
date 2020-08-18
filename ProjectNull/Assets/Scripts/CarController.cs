using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject CarControllerManager;

    void Start()
    {
        CarControllerManager.GetComponent<Car>().enabled = true;
    }

   
}
