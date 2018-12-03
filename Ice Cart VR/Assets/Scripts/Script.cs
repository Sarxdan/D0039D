using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Script : MonoBehaviour {

    public StandaloneInputModule InputModule;
    public EventSystem EventSystem;

	// Use this for initialization
	void Start ()
    {
        
        InputModule = this.gameObject.GetComponent<StandaloneInputModule>();
        // sets the input type to the first type of controller. (based on order in the machine) 
        string[] names = Input.GetJoystickNames();
        if (names.Length == 0)
        {
            InputModule.horizontalAxis  = "KeyboardHorizontal";
            InputModule.verticalAxis    = "KeyboardVertical";
            InputModule.submitButton    = "KeyboardSubmit";
            InputModule.cancelButton    = "KeyboardBack";
        }

        for (int i = 0; i < names.Length; i++)
        {
            // SteeringWheel
            if (names[i].Equals("G29 Driving Force Racing Wheel"))
            {
                InputModule.horizontalAxis  = "SteeringwheelHorizontal";
                InputModule.verticalAxis    = "SteeringwheelVertical";
                InputModule.submitButton    = "SteeringwheelSubmit";
                InputModule.cancelButton    = "SteeringwheelBack";
                break;
            }
            // Xbox Controller
            else if (names[i].Equals("Xbox One For Windows") || names[i].Equals("Controller (XBOX 360 For Windows)"))
            {

                InputModule.horizontalAxis  = "XboxHorizontal";
                InputModule.verticalAxis    = "XboxVertical";
                InputModule.submitButton    = "XboxSubmit";
                InputModule.cancelButton    = "XboxBack";
                break;
            }
            // Ps4 Controller
            else if (names[i].Equals("Wireless Controller"))
            {
                InputModule.horizontalAxis  = "Ps4Horizontal";
                InputModule.verticalAxis    = "Ps4Vertical";
                InputModule.submitButton    = "Ps4Submit";
                InputModule.cancelButton    = "Ps4Back";
                break;
            }
            else
            {
                InputModule.horizontalAxis  = "KeyboardHorizontal";
                InputModule.verticalAxis    = "KeyboardVertical";
                InputModule.submitButton    = "KeyboardSubmit";
                InputModule.cancelButton    = "KeyboardBack";
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}









