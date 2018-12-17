using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerGame : MonoBehaviour
{
    public InputManager.ControllerType inputType = InputManager.ControllerType.keyboard;

    public GameObject panel;
    public GameObject pause;
    public GameObject complete;


    public GameObject panel2;

    public GameObject car;
    public string pauseButtonName;
    public bool isTrackComplete = false;

	// Use this for initialization
	void Start ()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        inputType = car.GetComponent<InputManager>().inputType;

        if (inputType == InputManager.ControllerType.keyboard)
        {
            pauseButtonName = "KeyboardPause";
        }
        else if (inputType == InputManager.ControllerType.ps4Controller)
        {
            pauseButtonName = "Ps4Pause";
        }
        else if (inputType == InputManager.ControllerType.steeringWheel)
        {
            pauseButtonName = "SteeringwheelPause";
        }
        else if (inputType == InputManager.ControllerType.xboxController)
        {
            pauseButtonName = "XboxPause";
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        isTrackComplete = car.GetComponent<CheckpointScript>().isTrackComplete;
		if(Input.GetButton(pauseButtonName) && pause.active == false && isTrackComplete == false)
        {
            panel.active = true;
            pause.active = true;
            panel2.active = false;
            LogitechGSDK.LogiStopDirtRoadEffect(car.GetComponent<Car>().index);

            Time.timeScale = 0;
        }
        if (isTrackComplete)
        {
            panel.active = true;
            complete.active = true;
        }
	}

    public void Unpause()
    {
        pause.active = false;
        panel.active = false;
        panel2.active = true;
        Time.timeScale = 1;
    }
    public void Reset()
    {
        car.transform.position = car.GetComponent<CheckpointScript>().lastCheckpointPosition;
        car.transform.rotation = car.GetComponent<CheckpointScript>().lastCheckpointRotation;
        car.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        pause.active = false;
        panel.active = false;
        panel2.active = true;
        Time.timeScale = 1;
    }

    public void ToMainMenu()
    {
        Unpause();
        Destroy(car);
        SceneManager.LoadScene(0);
    }
}
