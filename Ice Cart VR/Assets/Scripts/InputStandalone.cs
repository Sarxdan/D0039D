using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine;

public class InputStandalone : MonoBehaviour {

    public StandaloneInputModule InputModule;
    public EventSystem EventSystem;
    public Dropdown controllerType, cameraType, gearType;
    public GameObject gameState;
    public int cameraIndex, controllerIndex, gearIndex;



	// Use this for initialization
	void Start ()
    {
        InputModule = this.gameObject.GetComponent<StandaloneInputModule>();
        gameState = GameObject.Find("GameState");

        if (gameState == null)
            Debug.Log("ERROR: No GameState Object in the scen.");
        else
        {
            controllerIndex = gameState.GetComponent<GameStateScript>().controllerType;
            cameraIndex = gameState.GetComponent<GameStateScript>().cameraType;
            // If the GameState object was empty. (first time game is started)
            if (controllerIndex == -1 || cameraIndex == -1)
            {
                // Get the names of all joysticks connected
                string[] names = Input.GetJoystickNames();
                if (names.Length == 0)
                { controllerIndex = 0; Debug.Log("No controllers connected."); }
                else
                    // Set current inputType to first connected joystick depending on the joysticks name
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (names[i].Equals("Controller (Xbox One For Windows)") || names[i].Equals("Controller (XBOX 360 For Windows)"))
                        { controllerIndex = 1; gearIndex = 0; break; }
                        else if (names[i].Equals("Wireless Controller"))
                        { controllerIndex = 2; gearIndex = 0; break; }
                        else if (names[i].Equals("G29 Driving Force Racing Wheel"))
                        { controllerIndex = 3; gearIndex = 1; break; }
                    }
                cameraIndex = 0;
                UpdateCameraType();
            }
            else
            {
                controllerIndex = gameState.GetComponent<GameStateScript>().controllerType;
                cameraIndex = gameState.GetComponent<GameStateScript>().cameraType;
                gearIndex = gameState.GetComponent<GameStateScript>().gearType;
            }
            controllerType.value = controllerIndex;
            cameraType.value = cameraIndex;
            gearType.value = gearIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameState.GetComponent<GameStateScript>().controllerType = controllerIndex;
        gameState.GetComponent<GameStateScript>().cameraType = cameraIndex;
        gameState.GetComponent<GameStateScript>().gearType = gearIndex;
    }

    public void UpdateControllerType()
    {
        controllerIndex = controllerType.value;
        if (controllerIndex == 0)
        {
            InputModule.horizontalAxis = "KeyboardHorizontal";
            InputModule.verticalAxis = "KeyboardVertical";
            InputModule.submitButton = "KeyboardSubmit";
            InputModule.cancelButton = "KeyboardBack";
        }
        else if (controllerIndex == 1)
        {
            InputModule.horizontalAxis = "XboxHorizontal";
            InputModule.verticalAxis = "XboxVertical";
            InputModule.submitButton = "XboxSubmit";
            InputModule.cancelButton = "XboxBack";
        }
        else if (controllerIndex == 2)
        {
            InputModule.horizontalAxis = "Ps4Horizontal";
            InputModule.verticalAxis = "Ps4Vertical";
            InputModule.submitButton = "Ps4Submit";
            InputModule.cancelButton = "Ps4Back";
        }
        else if (controllerIndex == 3)
        {
            InputModule.horizontalAxis = "SteeringwheelHorizontal";
            InputModule.verticalAxis = "SteeringwheelVertical";
            InputModule.submitButton = "SteeringwheelSubmit";
            InputModule.cancelButton = "SteeringwheelBack";
        }
        gameState.GetComponent<GameStateScript>().controllerType = controllerIndex;
    }

    public void UpdateCameraType()
    {
        if (cameraType.value == 0 && XRDevice.isPresent)
        {

            GameObject[] cameras = GameObject.FindGameObjectsWithTag("TPP cam");
            cameraIndex = 0;
            for(int i = 0; i<cameras.Length; i++)
            {
                cameras[i].SetActive(false);
            }
            gameState.GetComponent<GameStateScript>().cameraType = cameraIndex;
        }
        else if(cameraType.value == 1)
        {
            GameObject[] cameras = GameObject.FindGameObjectsWithTag("TPP cam");
            cameraIndex = 1;
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(true);
            }
            gameState.GetComponent<GameStateScript>().cameraType = cameraIndex;
        }
        else
        {
            cameraType.value = 1;
            UpdateCameraType();
        }
    }

    public void UpdateGearType()
    {
        gearIndex = gearType.value;
        gameState.GetComponent<GameStateScript>().gearType = gearIndex;
    }
}
