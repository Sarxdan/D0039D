using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine;

public class InputStandalone : MonoBehaviour {

    public StandaloneInputModule InputModule;
    public EventSystem EventSystem;
    public Dropdown controllerType, cameraType;
    public GameObject car;
    public int index, cameraIndex;



	// Use this for initialization
	void Start ()
    {
        InputModule = this.gameObject.GetComponent<StandaloneInputModule>();
        car = GameObject.FindGameObjectWithTag("Player");
        if (car == null)
        {
            // sets the input type to the first type of controller. (based on order in the machine) 
            string[] names = Input.GetJoystickNames();
            if (names.Length == 0)
            {
                InputModule.horizontalAxis = "KeyboardHorizontal";
                InputModule.verticalAxis = "KeyboardVertical";
                InputModule.submitButton = "KeyboardSubmit";
                InputModule.cancelButton = "KeyboardBack";
                controllerType.value = 4;
            }
            for (int i = 0; i < names.Length; i++)
            {
                // SteeringWheel
                if (names[i].Equals("G29 Driving Force Racing Wheel"))
                {
                    InputModule.horizontalAxis = "SteeringwheelHorizontal";
                    InputModule.verticalAxis = "SteeringwheelVertical";
                    InputModule.submitButton = "SteeringwheelSubmit";
                    InputModule.cancelButton = "SteeringwheelBack";
                    index = 3;
                    break;
                }
                // Xbox Controller
                else if (names[i].Equals("Xbox One For Windows") || names[i].Equals("Controller (XBOX 360 For Windows)"))
                {

                    InputModule.horizontalAxis = "XboxHorizontal";
                    InputModule.verticalAxis = "XboxVertical";
                    InputModule.submitButton = "XboxSubmit";
                    InputModule.cancelButton = "XboxBack";
                    index = 1;
                    break;
                }
                // Ps4 Controller
                else if (names[i].Equals("Wireless Controller"))
                {
                    InputModule.horizontalAxis = "Ps4Horizontal";
                    InputModule.verticalAxis = "Ps4Vertical";
                    InputModule.submitButton = "Ps4Submit";
                    InputModule.cancelButton = "Ps4Back";
                    index = 2;
                    break;
                }
                // Keyboard
                else
                {
                    InputModule.horizontalAxis = "KeyboardHorizontal";
                    InputModule.verticalAxis = "KeyboardVertical";
                    InputModule.submitButton = "KeyboardSubmit";
                    InputModule.cancelButton = "KeyboardBack";
                    index = 0;
                }
            }
            controllerType.value = index;
            cameraType.value = 0;
            UpdateCameraType();
        }
        else
        {
            index = car.GetComponent<Car>().controllerType;
            cameraIndex = car.GetComponent<Car>().cameraType;
            // SteeringWheel
            if (index == 3)
            {
                InputModule.horizontalAxis = "SteeringwheelHorizontal";
                InputModule.verticalAxis = "SteeringwheelVertical";
                InputModule.submitButton = "SteeringwheelSubmit";
                InputModule.cancelButton = "SteeringwheelBack";
                index = 3;
            }
            // Xbox Controller
            else if (index == 1)
            {

                InputModule.horizontalAxis = "XboxHorizontal";
                InputModule.verticalAxis = "XboxVertical";
                InputModule.submitButton = "XboxSubmit";
                InputModule.cancelButton = "XboxBack";
                index = 1;
            }
            // Ps4 Controller
            else if (index == 2)
            {
                InputModule.horizontalAxis = "Ps4Horizontal";
                InputModule.verticalAxis = "Ps4Vertical";
                InputModule.submitButton = "Ps4Submit";
                InputModule.cancelButton = "Ps4Back";
                index = 2;
            }
            // Keyboard
            else
            {
                InputModule.horizontalAxis = "KeyboardHorizontal";
                InputModule.verticalAxis = "KeyboardVertical";
                InputModule.submitButton = "KeyboardSubmit";
                InputModule.cancelButton = "KeyboardBack";
                index = 0;
            }
            controllerType.value = index;
            cameraType.value = cameraIndex;
            UpdateCameraType();
        }
    }

    // Update is called once per frame
    void Update()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        if (car != null)
        {
            car.GetComponent<Car>().controllerType = index;
            car.GetComponent<Car>().cameraType = cameraIndex;
        }
    }

    public void UpdateControllerType()
    {
        // Keyboard
        if (controllerType.value == 0)
        {
            InputModule.horizontalAxis = "KeyboardHorizontal";
            InputModule.verticalAxis = "KeyboardVertical";
            InputModule.submitButton = "KeyboardSubmit";
            InputModule.cancelButton = "KeyboardBack";
            index = 0;
        }
        // Xbox Controller
        else if (controllerType.value == 1)
        {

            InputModule.horizontalAxis = "XboxHorizontal";
            InputModule.verticalAxis = "XboxVertical";
            InputModule.submitButton = "XboxSubmit";
            InputModule.cancelButton = "XboxBack";
            index = 1;
        }
        // Ps4 Controller
        else if (controllerType.value == 2)
        {
            InputModule.horizontalAxis = "Ps4Horizontal";
            InputModule.verticalAxis = "Ps4Vertical";
            InputModule.submitButton = "Ps4Submit";
            InputModule.cancelButton = "Ps4Back";
            index = 2;
        }
        // SteeringWheel
        else if (controllerType.value == 3)
        {
            InputModule.horizontalAxis = "SteeringwheelHorizontal";
            InputModule.verticalAxis = "SteeringwheelVertical";
            InputModule.submitButton = "SteeringwheelSubmit";
            InputModule.cancelButton = "SteeringwheelBack";
            index = 3;
        }
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
        }
        else if(cameraType.value == 1)
        {
            GameObject[] cameras = GameObject.FindGameObjectsWithTag("TPP cam");
            cameraIndex = 1;
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(true);
            }
        }
        else
        {
            cameraType.value = 1;
            UpdateCameraType();
        }
    }
}
