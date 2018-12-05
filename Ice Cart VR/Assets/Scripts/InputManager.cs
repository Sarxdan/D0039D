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
            else if (names[i].Equals("Xbox One For Windows"))
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
        // Paddleshift means using only 2 buttons. One for shifting up, and one for shifting down
        if (shiftType == gearShiftType.paddleShift)
        {
            // Shift the gear up and down when the corresponding buttons are pressed
            // GetButtonDown is only called on the first frame the button is pressed
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

    void FixedUpdate()
    {
        // H-shift (Currently only works with logitech stick-shift)
        // H-shift means you have one button per gear
        if (shiftType == gearShiftType.hShift)
        {
            // Set the correct gear depending on the gear-button pressed
            // GetAxis returns a value between 0 and 1 for the stickshift
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
        }
        else if (shiftType == gearShiftType.automatic)
        {
            // Needs more code because it doesn't support reversing or neutral gear
            // Gets the ideal gear calculated by the car-script
            gear = carScript.getIdealGear();
        }
    }

    // Horizontal input used for steering
    public float getHorizontal()
    {
        // Depending on the ControllerType, use the correct input
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

    // Gas input used for accelerating
    public float getGas()
    {
        // Depending on the ControllerType, use the correct input
        if (inputType == ControllerType.keyboard)
        {
            // KeybordVertical has both a positive and negative button
            // Make sure the gas is only returned when positive, otherwise return 0 (no acceleration)
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
            // This input has a value between -1 and 1 but we want it between 0 and 1, therefore the (.. + 1) / 2
            return (Input.GetAxis("Ps4Gas") + 1) / 2;
        }
        else if (inputType == ControllerType.steeringWheel)
        {
            // This input has a value between -1 and 1 but we want it between 0 and 1, therefore the (.. + 1) / 2
            return (Input.GetAxis("Steeringwheel-gas") + 1) / 2;
        }

        return 0;
    }

    // Brake input used for braking
    public float getBrake()
    {
        // Depending on the ControllerType, use the correct input
        if (inputType == ControllerType.keyboard)
        {
            // KeybordVertical has both a positive and negative button
            // Make sure the brake is only returned when input is negative
            // If it is negative, return the positive version of the number, otherwise return 0 (no braking)
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
            // This input has a value between -1 and 1 but we want it between 0 and 1, therefore the (.. + 1) / 2
            return (Input.GetAxis("Ps4Brake") + 1) / 2;
        }
        else if (inputType == ControllerType.steeringWheel)
        {
            // This input has a value between -1 and 1 but we want it between 0 and 1, therefore the (.. + 1) / 2
            return (Input.GetAxis("Steeringwheel-brake") + 1) / 2;
        }

        return 0;
    }

    // Clutch input used for gearing up and down
    public float getClutch()
    {
        // Depending on the ControllerType, use the correct input
        if (inputType == ControllerType.steeringWheel)
        {
            // This input has a value between -1 and 1 but we want it between 0 and 1, therefore the (.. + 1) / 2
            return (Input.GetAxis("Steeringwheel-clutch") + 1) / 2;
        }

        // If no clutch is connected, return 0
        // This means H-shift will be broken
        return 0;
    }

    // Returns gear as calculated in Update and FixedUpdate
    public int getGear()
    {
        // Make sure the gear is within range
        if (gear < -1)
        {
            gear = -1;
        }
        else if (gear > 6)
        {
            gear = 6;
        }

        return gear;
    }

}
