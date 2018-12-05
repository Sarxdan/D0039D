using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerGame : MonoBehaviour
{
    public Car.ControllerType inputType = Car.ControllerType.keyboard;

    public GameObject pause;
    public GameObject panel;
    public GameObject car;
    public string pauseButtonName;

	// Use this for initialization
	void Start ()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        inputType = car.GetComponent<Car>().inputType;

        if (inputType == Car.ControllerType.keyboard)
        {
            pauseButtonName = "KeyboardPause";
        }
        else if (inputType == Car.ControllerType.ps4Controller)
        {
            pauseButtonName = "Ps4Pause";
        }
        else if (inputType == Car.ControllerType.steeringWheel)
        {
            pauseButtonName = "SteeringwheelPause";
        }
        else if (inputType == Car.ControllerType.xboxController)
        {
            pauseButtonName = "XboxPause";
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetButton(pauseButtonName) && pause.active == false)
        {
            panel.active = true;
            pause.active = true;
            Time.timeScale = 0;
        }
	}

    public void Unpause()
    {
        pause.active = false;
        panel.active = false;
        Time.timeScale = 1;
    }
    public void Reset()
    {
        car.transform.position = car.GetComponent<CheckpointScript>().lastCheckpointPosition;
        car.transform.rotation = car.GetComponent<CheckpointScript>().lastCheckpointRotation;
        car.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        pause.active = false;
        panel.active = false;
        Time.timeScale = 1;
    }
}
