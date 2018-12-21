using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputManager : MonoBehaviour {

    public enum ControllerType { keyboard, xboxController, ps4Controller, steeringWheel };
    public ControllerType inputType;
    public enum gearShiftType { paddleShift, hShift, automatic };
    public gearShiftType shiftType = gearShiftType.paddleShift;
    private Car carScript;
    private GameStateScript gameState;

    private int gear = 0;
    private int lastGear = 0;
    private bool failedGearShift = false;

    void Start ()
    {
        // Store main car script and the gameState script.
        carScript = GetComponent<Car>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateScript>();
        // Bindes the correct input type dependent on the gameState.
        if (gameState.controllerType == 0)
        {
            inputType = ControllerType.keyboard;
        }
        else if (gameState.controllerType == 1)
        {
            inputType = ControllerType.xboxController;
        }
        else if (gameState.controllerType == 2)
        {
            inputType = ControllerType.ps4Controller;
        }
        else if (gameState.controllerType == 3)
        {
            inputType = ControllerType.steeringWheel;
        }
        // Bindes the correct gear type dependent of the gameState.
        if (gameState.gearType == 0)
        {
            shiftType = gearShiftType.automatic;
        }
        else if (gameState.gearType == 1)
        {
            shiftType = gearShiftType.hShift;
        }
        else if (gameState.gearType == 2)
        {
            shiftType = gearShiftType.paddleShift;
        }

    }


    void Update()
    {
        UpdateInputType();
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

    // Vertical input
    public float getVertical()
    {
        // Depending on the ControllerType, use the correct input
        if (inputType == ControllerType.keyboard)
        {
            return Input.GetAxis("KeyboardVertical");
        }
        else if (inputType == ControllerType.xboxController)
        {
            return Input.GetAxis("XboxVertical");
        }
        else if (inputType == ControllerType.ps4Controller)
        {
            return Input.GetAxis("Ps4Vertical");
        }
        else if (inputType == ControllerType.steeringWheel)
        {
            return Input.GetAxis("SteeringwheelVertical");
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
        // If you are trying to shift gears using h-shift
        if (shiftType == gearShiftType.hShift)
        {
            // If the clutch is pushed down enough and you have not already failed the gearshift.
            if (getClutch() >= 0.8f && !failedGearShift)
            {
                lastGear = gear;
                return gear;
            }
            else if (gear == 0)
            {
                // If the car is put in neutral by the user, remove all eventual engine braking.
                failedGearShift = false;
                lastGear = gear;
                return gear;
            }
            else if (gear == lastGear)
            {
                return gear;
            }
            else
            {
                // If you don't push down the clutch, put the car in neutral instead and tell the car to engine brake.
                failedGearShift = true;
                return 0;
            }
        }

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

    public bool getFailedGearShift()
    {
        return failedGearShift;
    }

    public void UpdateInputType()
    {
        if (gameState.controllerType == 0)
        {
            inputType = ControllerType.keyboard;
        }
        else if (gameState.controllerType == 1)
        {
            inputType = ControllerType.xboxController;
        }
        else if (gameState.controllerType == 2)
        {
            inputType = ControllerType.ps4Controller;
        }
        else if (gameState.controllerType == 3)
        {
            inputType = ControllerType.steeringWheel;
        }
        // Bindes the correct gear type dependent of the gameState.
        if (gameState.gearType == 0)
        {
            shiftType = gearShiftType.automatic;
        }
        else if (gameState.gearType == 1)
        {
            shiftType = gearShiftType.hShift;
        }
        else if (gameState.gearType == 2)
        {
            shiftType = gearShiftType.paddleShift;
        }
    }
}
