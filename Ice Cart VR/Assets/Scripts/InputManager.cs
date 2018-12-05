using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputManager : MonoBehaviour {

    public enum ControllerType { keyboard, xboxController, ps4Controller, steeringWheel };
    public ControllerType inputType = ControllerType.keyboard;
    public enum gearShiftType { paddleShift, hShift, automatic };
    public gearShiftType shiftType = gearShiftType.paddleShift;
    private Car carScript;

    private int gear = 0;

    void Start ()
    {
        // Store main car script
        carScript = GetComponent<Car>();

        // Get the names of all joysticks connected
        string[] names = Input.GetJoystickNames();
        // Set current inputType to first connected joystick depending on the joysticks name
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Equals("G29 Driving Force Racing Wheel"))
            {
                inputType = ControllerType.steeringWheel;
            }
            else if (names[i].Equals("Xbox One For Windows") || names[i].Equals("Controller (XBOX 360 For Windows)"))
            {
                inputType = ControllerType.xboxController;
            }
            else if (names[i].Equals("Wireless Controller"))
            {
                inputType = ControllerType.ps4Controller;
            }
        }
    }


    void Update()
    {
        // Paddleshift (Currently only supports Xbox-controller)
        // 
        if (shiftType == gearShiftType.paddleShift)
        {
            if (Input.GetButtonDown("XboxSubmit"))
            {
                gear++;
            }

            if (Input.GetButtonDown("XboxBack"))
            {
                gear--;
            }
        }
    }
    private void FixedUpdate()
    {
        if (shiftType == gearShiftType.hShift)
        {
            if (Input.GetAxis("Steeringwheel-gear1") >= 0.75)
            {
                gear = 1;
            }
            else if (Input.GetAxis("Steeringwheel-gear2") >= 0.75)
            {
                gear = 2;
            }
            else if (Input.GetAxis("Steeringwheel-gear3") >= 0.75)
            {
                gear = 3;
            }
            else if (Input.GetAxis("Steeringwheel-gear4") >= 0.75)
            {
                gear = 4;
            }
            else if (Input.GetAxis("Steeringwheel-gear5") >= 0.75)
            {
                gear = 5;
            }
            else if (Input.GetAxis("Steeringwheel-gear6") >= 0.75)
            {
                gear = 6;
            }
            else if (Input.GetAxis("Steeringwheel-gearReverse") >= 0.75)
            {
                gear = -1;
            }
            else
            {
                gear = 0;
            }
            Debug.Log("input : " + Input.GetAxis("Steeringwheel-gear1"));
            Debug.Log("gear : " + gear);
        }
        else if (shiftType == gearShiftType.automatic)
        {
            // Needs more code because it doesn't support reversing or neutral gear
            gear = carScript.getIdealGear();
        }

        if (gear < -1)
        {
            gear = -1;
        }
        else if (gear > 6)
        {
            gear = 6;
        }
    }

    public float getHorizontal()
    {
        if (inputType == ControllerType.keyboard)
        {
            return Input.GetAxis("KeyboardHorizontal");

        }
        else if (inputType == ControllerType.xboxController)
        {
            return Input.GetAxis("XboxHorizontal");
        }
        else if (inputType == ControllerType.ps4Controller)
        {
            return Input.GetAxis("Ps4Horizontal");

        }
        else if (inputType == ControllerType.steeringWheel)
        {
            return Input.GetAxis("Steeringwheel-wheel");
        }

        return 0;
    }

    public float getGas()
    {
        if (inputType == ControllerType.keyboard)
        {
            if (Input.GetAxis("KeyboardVertical") > 0)
            {
                return Input.GetAxis("KeyboardVertical");
            }

            return 0;
        }
        else if (inputType == ControllerType.xboxController)
        {
            return Input.GetAxis("XboxGas");
        }
        else if (inputType == ControllerType.ps4Controller)
        {
            return (Input.GetAxis("Ps4Gas") + 1) / 2;
        }
        else if (inputType == ControllerType.steeringWheel)
        {
            return (Input.GetAxis("Steeringwheel-gas") + 1) / 2;
        }

        return 0;
    }

    public float getBrake()
    {
        if (inputType == ControllerType.keyboard)
        {
            if (Input.GetAxis("KeyboardVertical") < 0)
            {
                return -Input.GetAxis("KeyboardVertical");
            }

            return 0;
        }
        else if (inputType == ControllerType.xboxController)
        {
            return Input.GetAxis("XboxBrake");
        }
        else if (inputType == ControllerType.ps4Controller)
        {
            return (Input.GetAxis("Ps4Brake") + 1) / 2;
        }
        else if (inputType == ControllerType.steeringWheel)
        {
            return (Input.GetAxis("Steeringwheel-brake") + 1) / 2;
        }

        return 0;
    }

    public float getClutch()
    {
        if (inputType == ControllerType.steeringWheel)
        {
            return (Input.GetAxis("Steeringwheel-clutch") + 1) / 2;
        }

        return 0;
    }

    public int getGear()
    {
        return gear;
    }

}
