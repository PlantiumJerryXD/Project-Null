using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRace : MonoBehaviour
{
    public GameObject MyCar;
    public GameObject FinishCam;
    public GameObject ViewModes;


    void OnTriggerEnter()
    {
        MyCar.SetActive(false);
        MyCar.GetComponent<CarController>().enabled = false;
        MyCar.GetComponent<Car>().enabled = false;
        MyCar.SetActive(true);
        FinishCam.SetActive(true);
        ViewModes.SetActive(false);

    }


}

