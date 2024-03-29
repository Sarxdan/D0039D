﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine;

public class Car : MonoBehaviour
{
    // WheelFrictionCurve used to set the wheelfriction on diffrent surfaces.
    public WheelFrictionCurve ff;                       // ForwardFriction
    public WheelFrictionCurve sf;                       // SidewardFriction

    // Input related variables.
    public int steeringWheelIndex = 0;                  // Index of the SteeringWheel. (used for forcefeedback)
    public int driveType = 0;                           // Type of Drive.
    public float horizontalInput;                       // Input
    public float verticalInput;                         // Input
    public float gasInput = 0.0F;                       // Input
    public float brakeInput = 0.0F;                     // Input
    public float clutchInput = 0.0F;                    // Input
    public float backInput = 0.0F;                      // Input
    public float submitInput = 0.0F;                    // Input
    public int controllerType = -1;                     // Controller type.
    public int cameraType = -1;                         // Camera Type.

    // ? 
    private new Rigidbody rigidbody;                    // The cars rigidbody. (used to change the center of mass)
    public InputManager inputScript;                    // Script to call all the input functions.
    public GameStateScript gameState;

    // Tyre shape and colliders for all the wheel.
    public GameObject wheelShape;                       // GameObject to lock in the shape of the wheel.
    public WheelCollider[] wheels;                      // array of all wheels.
    public WheelCollider[] frontWheels;                 // array of the front wheels.
    public WheelCollider[] rearWheels;                  // array of the rear wheels.

    // Cosmetics.
    public GameObject steeringWheel, acceleratorPad, breakPad, clutchPad;       // Cosmetics
    public Text speedDisplay;                           // Ui display of the current speed.
    public Text gearDisplay;                            // Ui display of the current gear.
    public GameObject needle;                           // Ui gameobject. (a needle that rotates)
    public Canvas ui;                                   // Canvas. (to remove the ui when not in use) 

    // Set values for diffrent things. 
    public float maxSteeringAngle = 10;                 // max angle the wheels can turn.
    public float maxSteeringWheelRot = 450;             // max degrees the steeringwheel can turn. (in one direction, + and - )
    public float maxPedalPress = 30;                    // max degrees the pedals can be pressed down.
    public float enginePower = 300;                     // max power the engine can put out.
    public float slowDownForce = 100.0f;                // the force pushing the car in the opposite direction.
    public float antiRollSpring = 0;                    // spring force is used to stableize the car.
    public int   gear = 0;                              // current gear.
    public float velocityZ = 0;                         // current speed in z-axis.
    public int   velocityZInt;                          // current speed in z-axis as int.
    public float RPM = 0;                               // current RPM of motor. (between 0-1) (sudo 1000-3000 rpm)
    public int groundType;

    public bool includeChildren = true;                 // ? /// WHAT IS THIS \\\

    public GameObject tppCam;


    // The distance between gears in units per second.
    public float gearDistance = 2.2f;                   // speed needed to shift gear. (1.1 - 3.3 - 5.5 - 7.7 ...)

    // Empty, if not in test scen then cast Init.
    void Start()
    {
        //Init();                                       // Needed to run test scen.
    }

    // Init the car script, finding and making all the necessery game objects.
    public void Init()
    {
        // sets Camera to enable toggle between TPP and FPP.
        tppCam = GameObject.Find("TPP Cam");

        // Sets the steeringwheel index.
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetJoystickNames()[i] == "G29 Driving Force Racing Wheel")
                steeringWheelIndex = i;
        }
        // Remove logitechsteringSDK if the Logitechsteringwheel is not connected.
        if (LogitechGSDK.LogiIsConnected(steeringWheelIndex))
        {
            GameObject.Find("FPP Cam").GetComponent<LogitechSteeringWheel>().enabled = false;
        }

        // Sets Gamestate.
        gameState = GameObject.Find("GameState").GetComponent<GameStateScript>();
        if (gameState.cameraType == 0)
        {
            tppCam.SetActive(false);
        }

        // Sets inputScript. 
        inputScript = GetComponent<InputManager>();
        inputScript.enabled = true;                                 // activates the input, used to make the menu not error out.

        // Sets rigidbody
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = new Vector3(0, 0.139f, 0.1f);      // Hard coding to change the center of mass to make the car more stable.

        // Binds cosmetic objects (used to rotate the wheel and pedals)
        steeringWheel   = GameObject.FindWithTag("SteeringWheel");
        acceleratorPad  = GameObject.FindWithTag("AccelleratorPad");
        breakPad        = GameObject.FindWithTag("BreakPad");
        clutchPad       = GameObject.FindWithTag("ClutchPad");

        // Get all the Wheel Colliders for the car
        wheels = GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelCollider thisWheel = wheels[i];
            // Changes the frequency of updates.
            thisWheel.ConfigureVehicleSubsteps(500, 450, 500);

            // Adds the wheel prefab to the car.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = thisWheel.transform;
            }
        }

        // Divide wheels into front and rear wheels

        // Number of front wheels. Used to decide sizes of arrays
        int numOfFrontWheels = 0;
        // Loop to determine number of front wheels
        foreach (WheelCollider wheel in wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                numOfFrontWheels++;
            }
        }

        frontWheels = new WheelCollider[numOfFrontWheels];
        rearWheels = new WheelCollider[wheels.Length - numOfFrontWheels];

        // Indexes used to allow multiple tires at each axis (for example two rear right tires and two rear left tires)
        int currentRearLeftIndex = 0;
        int currentRearRightIndex = 1;
        int currentFrontLeftIndex = 0;
        int currentFrontRightIndex = 1;

        // Insert front and rear wheels into their respective arrays where left wheels are even numbers and right wheels are odd numbers.
        foreach (WheelCollider wheel in wheels)
        {
            // Checks if position of wheel is infront of local midpoint.
            if (wheel.transform.localPosition.z < 0)
            {
                // Checks if positon of wheel is to the left of local midline.
                if (wheel.transform.localPosition.x < 0)
                {
                    // Rear left tire
                    rearWheels[currentRearLeftIndex] = wheel;
                    currentRearLeftIndex += 2;
                }
                else
                {
                    // Rear right tire
                    rearWheels[currentRearRightIndex] = wheel;
                    currentRearRightIndex += 2;
                }
            }
            else
            {
                // Checks if positon of wheel is to the right of local midline.
                if (wheel.transform.localPosition.x < 0)
                {
                    // Front left tire
                    frontWheels[currentFrontLeftIndex] = wheel;
                    currentFrontLeftIndex += 2;
                }
                else
                {
                    // Front right tire
                    frontWheels[currentFrontRightIndex] = wheel;
                    currentFrontRightIndex += 2;
                }
            }
        }
    }
    void UpdateLogitech()
    {
        Debug.Log("logi");
        //LogitechGSDK.LogiPlaySpringForce(steeringWheelIndex, 0, 100, 10);
        //*velocityZInt
        if (groundType == 1)
        {
            Debug.Log(groundType);
            LogitechGSDK.LogiStopDirtRoadEffect(steeringWheelIndex);
            LogitechGSDK.LogiStopDamperForce(steeringWheelIndex);

            LogitechGSDK.LogiPlaySlipperyRoadEffect(steeringWheelIndex, 50);
        }
        else if (groundType == 2)
        {
            Debug.Log(groundType);
            LogitechGSDK.LogiStopDirtRoadEffect(steeringWheelIndex);
            LogitechGSDK.LogiStopSlipperyRoadEffect(steeringWheelIndex);

            //LogitechGSDK.LogiPlayDamperForce(steeringWheelIndex, 50);
        }
        else if (groundType == 3)
        {
            Debug.Log(groundType);
            LogitechGSDK.LogiStopSlipperyRoadEffect(steeringWheelIndex);
            LogitechGSDK.LogiStopDamperForce(steeringWheelIndex);

            LogitechGSDK.LogiPlayDirtRoadEffect(steeringWheelIndex, 10 * velocityZInt);
        }

    }

    // Runs the update each frame.
    void FixedUpdate()
    {
        // Updates all the needed functions.
        GetInput();                                                         // Takes input.
        UpdateOptions();
        LogitechGSDK.LogiUpdate();                                          // Function needed for LogitechGSDK force feedback.
        RotateSteeringWheel(steeringWheel);                                 // Cosmetic rotation of the wheel.
        PressPedals(acceleratorPad, breakPad, clutchPad);                   // Cosmetic pushing of the pedals.
        CalculateVisualComponents();                                        // Cosmetic calculation of RPM and speed.


        // Forcefeedback to center the wheel, depending on speed.
        LogitechGSDK.LogiPlaySpringForce(steeringWheelIndex,0,100,10*velocityZInt);

        // Goes through all the WheelColliders.
        foreach (WheelCollider wheel in wheels)
        {
            // Checks if the wheel is on a new surface.
            WheelHit hit;
            if (wheel.GetGroundHit(out hit))
            {
                // Modifier value gathered from wheel prefab
                WheelModifier wheelMod = wheel.transform.GetChild(0).GetComponent<WheelModifier>();

                // Changes the effectivness of the wheel depending on the material
                if (hit.collider.tag == "ice")
                {
                    groundType = 1;
                    UpdateLogitech();
                    ff.asymptoteSlip = 0.4f     * wheelMod.forwardFrictionMod;
                    ff.asymptoteValue = 0.2f    * wheelMod.forwardFrictionMod;
                    ff.extremumSlip = 2.0f      * wheelMod.forwardFrictionMod;
                    ff.extremumValue = 0.8f     * wheelMod.forwardFrictionMod;
                    ff.stiffness = 3.5f         * wheelMod.forwardFrictionMod;
                    sf.asymptoteSlip = 0.2f     * wheelMod.sidewaysFrictionMod;
                    sf.asymptoteValue = 0.2f    * wheelMod.sidewaysFrictionMod;
                    sf.extremumSlip = 0.6f      * wheelMod.sidewaysFrictionMod;
                    sf.extremumValue = 0.5f     * wheelMod.sidewaysFrictionMod;
                    sf.stiffness = 2.1f         * wheelMod.sidewaysFrictionMod;
                    wheel.forwardFriction = ff;
                    wheel.sidewaysFriction = sf;

                    slowDownForce = 0.1f        * wheelMod.resistanceMod;
                }
                else if (hit.collider.tag == "tarmac")
                {
                    groundType = 2;
                    UpdateLogitech();
                    ff.asymptoteSlip = 0.7f     * wheelMod.forwardFrictionMod;
                    ff.asymptoteValue = 0.7f    * wheelMod.forwardFrictionMod;
                    ff.extremumSlip = 7.7f      * wheelMod.forwardFrictionMod;
                    ff.extremumValue = 8.4f     * wheelMod.forwardFrictionMod;
                    ff.stiffness = 8.4f         * wheelMod.forwardFrictionMod;
                    sf.asymptoteSlip = 0.4f     * wheelMod.sidewaysFrictionMod;
                    sf.asymptoteValue = 0.6f    * wheelMod.sidewaysFrictionMod;
                    sf.extremumSlip = 4.4f      * wheelMod.sidewaysFrictionMod;
                    sf.extremumValue = 6.8f     * wheelMod.sidewaysFrictionMod;
                    sf.stiffness = 4.8f         * wheelMod.sidewaysFrictionMod;
                    wheel.forwardFriction = ff;
                    wheel.sidewaysFriction = sf;

                    slowDownForce = 50.0f       * wheelMod.resistanceMod;
                }
                else if (hit.collider.tag == "dirt")
                {
                    groundType = 3;
                    UpdateLogitech();
                    ff.asymptoteSlip = 0.7f     * wheelMod.forwardFrictionMod;
                    ff.asymptoteValue = 0.7f    * wheelMod.forwardFrictionMod;
                    ff.extremumSlip = 7.7f      * wheelMod.forwardFrictionMod;
                    ff.extremumValue = 8.4f     * wheelMod.forwardFrictionMod;
                    ff.stiffness = 8.4f         * wheelMod.forwardFrictionMod;
                    sf.asymptoteSlip = 0.4f     * wheelMod.sidewaysFrictionMod;
                    sf.asymptoteValue = 0.4f    * wheelMod.sidewaysFrictionMod;
                    sf.extremumSlip = 4.4f      * wheelMod.sidewaysFrictionMod;
                    sf.extremumValue = 3.6f     * wheelMod.sidewaysFrictionMod;
                    sf.stiffness = 4.8f         * wheelMod.sidewaysFrictionMod;
                    wheel.forwardFriction = ff;
                    wheel.sidewaysFriction = sf;

                    slowDownForce = 100.0f      * wheelMod.resistanceMod;
                }

                // Apply natural slowdown
                rigidbody.AddForce(rigidbody.velocity.normalized * -slowDownForce);
            }
        }

        // Call axis stabilizer function using only the first two wheels in each axis
        StabilizeAxis(frontWheels[0], frontWheels[1]);                      // Front axis
        StabilizeAxis(rearWheels[0], rearWheels[1]);                        // Rear axis

        /// HERE THE WHEEL DRIVE HAVE TO MAKE A DIFFRENCE \\\
        // Front wheels
        foreach (var wheel in frontWheels)
        {
            //Accelerate(wheel);
            Steer(wheel);
            
        }
        // Rear wheels
        foreach (var wheel in rearWheels)
        {
            Brake(wheel);
            Accelerate(wheel);
        }

        // Update positions of wheels cosmetic
        foreach (WheelCollider wheel in wheels)
        {
            UpdateWheelPoses(wheel);
        }
    }

    public void UpdateOptions()
    {   
        // Camera option.
        if (gameState.cameraType == 0 && tppCam.activeSelf)
        {
            tppCam.SetActive(false);
            GameObject.Find("FPP Cam").GetComponent<LogitechSteeringWheel>().enabled = true;
        }
        else if (gameState.cameraType != 0 && !tppCam.activeSelf)
        {
            tppCam.SetActive(true);
        }

        // Controller Options.

        // Gear Options.
        if (gameState.gearType == 0)
        {
            inputScript.shiftType = InputManager.gearShiftType.automatic;
        }
        else if (gameState.gearType == 1)
        {
            inputScript.shiftType = InputManager.gearShiftType.hShift;
        }
        else if (gameState.gearType == 2)
        {
            inputScript.shiftType = InputManager.gearShiftType.paddleShift;
        }
    }

    // Get input from controller.
    void GetInput()
    {
        horizontalInput = inputScript.getHorizontal();
        verticalInput = inputScript.getVertical();
        gasInput = inputScript.getGas();
        brakeInput =  inputScript.getBrake();
        clutchInput = inputScript.getClutch();
        gear = inputScript.getGear();
    }

    // Only cosmetic rotation of the steeringwheel model.
    void RotateSteeringWheel(GameObject steeringWheel)
    {
        steeringWheel.transform.localEulerAngles = new Vector3(0, -horizontalInput * maxSteeringWheelRot, 0);
    }

    // Only cosmetic pressing of the pedals.
    void PressPedals(GameObject accelerator, GameObject breaker, GameObject clutch)
    {
        accelerator.transform.localEulerAngles = new Vector3(-140 + gasInput * maxPedalPress, 0, 0);
        breaker.transform.localEulerAngles = new Vector3(-140 + brakeInput * maxPedalPress, 0, 0);
        clutch.transform.localEulerAngles = new Vector3(-140 + clutchInput * maxPedalPress, 0, 0);
    }

    // Rotate the wheels when steering.
    void Steer(WheelCollider wheel)
    {
        wheel.steerAngle = horizontalInput * maxSteeringAngle;
    }

    // Calculates the Rpm, SpeedDisplay, GearDisplay. (UI)
    void CalculateVisualComponents()
    {
        // First loop.
        if (speedDisplay == null)
        {
            speedDisplay = GameObject.Find("SpeedDisplay").GetComponent<Text>();
            gearDisplay = GameObject.Find("GearDisplay").GetComponent<Text>();
            needle = GameObject.FindWithTag("SpeedNeedle");
        }

        // The velocity in positive z direction of the car.
        velocityZ = transform.InverseTransformDirection(rigidbody.velocity).z;
        // Velocity is always positive.
        if (velocityZ < 0)
            velocityZ = -velocityZ;
        // Transform to int to display on speedomiter. also makes the number somewhat more realistic.
        int velocityAsInt = (int)((velocityZ * 3) * 3.6);
        // Transform to string to be displayed.
        string velocityAsString = velocityAsInt.ToString();
        speedDisplay.text = velocityAsString;

        // GearDisplay
        if (gear == -1)
            gearDisplay.text = "R";
        else if (gear == 0)
            gearDisplay.text = "N";
        else
            gearDisplay.text = gear.ToString();

        // RPM calculations.
        if (gear == 1)
            RPM = (velocityZ / (gearDistance / 2));                     // Gear 1 is half gearDistance.
        else if (gear == 2)
            RPM = (velocityZ - (gearDistance / 2)) / (gearDistance);    // Gear 2 have to remove first gearDistance.
        else
            RPM = (velocityZ - (gearDistance / 2) - (gearDistance * (gear - 2))) / gearDistance; // All other gears removes first and all gear before.
        
        // if the gear in wrong red shift.
        if (RPM < 0)
            RPM = 0;

        float degree = 170 - 210 * RPM;
        if (degree < -80)
            degree = -80;
        needle.transform.localEulerAngles = new Vector3(0, 0, degree);
    }

    //Make the car move according to the input
    void Accelerate(WheelCollider wheel)
    {
        // The velocity in positive z direction of the car.
        float zVel = transform.InverseTransformDirection(rigidbody.velocity).z;

        // Generates a -x^2 curve where velocity is x and y is torque output. The curve is moved in the x-axis depending on the gear and gearDistance
        float motorTorque = -((zVel - (Mathf.Abs(gear) - 1) * gearDistance) * (zVel - (Mathf.Abs(gear) - 1) * gearDistance) * (enginePower / (gearDistance * gearDistance))) + gasInput * enginePower;

        // Engine braking for all gears except neutral when in the wrong gear or failed gearshift.
        if ((inputScript.getFailedGearShift() || motorTorque < 0) && gear != 0)
        {
            motorTorque = 0;
            // Engine braking
            wheel.brakeTorque += enginePower / 4;
        }
        else if (gear == 0)
        {
            // Remove all motorTorque when in neutral
            motorTorque = 0;
        }
      
        if (gear < 0)
        {
            // Makes sure reversing gets same negative torque as its positive gear counterpart
            motorTorque = -motorTorque;
            if (motorTorque > 0)
            {
                motorTorque = 0;
            }
        }

        wheel.motorTorque = motorTorque;
    }

    // Gets the ideal gear to be used for automatic shifting
    public int getIdealGear()
    {
        // Finds the gear that should output the most power. 
        // Uses the gearDistance, meaning the distance in velocity between gears
        float zVel = transform.InverseTransformDirection(rigidbody.velocity).z;
        return (int)Mathf.Ceil((zVel + 1) / gearDistance);
    }

    void Brake(WheelCollider wheel)
    {
        wheel.brakeTorque = brakeInput * enginePower * 2;
    }

    //Updates the position of the wheel prefabs according to the wheel colliders
    void UpdateWheelPoses(WheelCollider wheel)
    {
        //Assume that the wheelcolliders first child is the wheel prefab
        Transform shapeTransform = wheel.transform.GetChild(0);

        Vector3 pos = shapeTransform.position;
        Quaternion quat = shapeTransform.rotation;

        wheel.GetWorldPose(out pos, out quat);

        shapeTransform.position = pos;
        shapeTransform.rotation = quat;

        // if right side wheel, flip the wheel 180.
        if (wheel.transform.localPosition.x > 0)
            shapeTransform.Rotate(new Vector3(0, 180, 0), Space.Self);
    }

    // Simulating stabilizer bars
    void StabilizeAxis(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit = new WheelHit();
        
        // Travel means the travel of the suspension
        float leftTravel = 0.5f;
        float rightTravel = 0.5f;

        // Check if wheel is touching the ground
        bool leftGrounded = leftWheel.isGrounded;
        leftWheel.GetGroundHit(out hit);

        // Update travel if wheel is touching the ground
        if (leftGrounded)
        {
            leftTravel = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
        }

        // Check if wheel is touching the ground
        bool rightGrounded = rightWheel.isGrounded;
        rightWheel.GetGroundHit(out hit);

        // Update travel if wheel is touching the ground
        if (rightGrounded)
        {
            rightTravel = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;
        }

        // Calculate the force to apply to each axis
        float antiRollForce = (leftTravel - rightTravel) * antiRollSpring;

        // Add force to wheels if wheel is grounded
        if (leftGrounded)
        {
            rigidbody.AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);
        }
        // Add force to wheels if wheel is grounded
        if (rightGrounded)
        {
            rigidbody.AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
        }
    }
}
