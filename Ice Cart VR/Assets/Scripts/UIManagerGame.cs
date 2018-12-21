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
		if(Input.GetButton(pauseButtonName) && pause.activeSelf == false && isTrackComplete == false)
        {
            panel.SetActive(true);
            pause.SetActive(true);
            panel2.SetActive(false);
            LogitechGSDK.LogiStopDirtRoadEffect(car.GetComponent<Car>().index);

            Time.timeScale = 0;
        }
        if (isTrackComplete)
        {
            panel.SetActive(true);
            panel2.SetActive(false);
            complete.SetActive(true);
        }
	}

    public void Unpause()
    {
        UpdatePauseButton();
        pause.SetActive(false);
        panel.SetActive(false);
        panel2.SetActive(true);
        Time.timeScale = 1;
    }
    public void Reset()
    {
        car.transform.position = car.GetComponent<CheckpointScript>().lastCheckpointPosition;
        car.transform.rotation = car.GetComponent<CheckpointScript>().lastCheckpointRotation;
        car.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        pause.SetActive(false);
        panel.SetActive(false);
        panel2.SetActive(true);
        Time.timeScale = 1;
    }

    public void ToMainMenu()
    {
        Unpause();
        Destroy(car);
        SceneManager.LoadScene(0);
    }
    public void UpdatePauseButton()
    {
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
}
