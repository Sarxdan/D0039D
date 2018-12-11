﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class Car : MonoBehaviour
{

    // Enum used to make choices easy.
    public enum wheelDrive { four, front, rear };
    public enum ControllerType { keyboard, xboxController, ps4Controller, steeringWheel };

    // WheelFrictionCurve used to set the wheelfriction on diffrent surfaces.
    public WheelFrictionCurve ff;
    public WheelFrictionCurve sf;

    // Input related variables.
    public int index = 0;
    public wheelDrive drive = wheelDrive.front;
    public float horizontalInput;
    public float verticalInput;
    public float gasInput = 0.0F;
    public float brakeInput = 0.0F;
    public float clutchInput = 0.0F;
    public float backInput = 0.0F;
    public float submitInput = 0.0F;

    // ? 
    private new Rigidbody rigidbody;
    private InputManager inputScript;
   
    // used to change the steeringangle of the wheel. 
    private float steeringAngle;

    // Tyre shape and colliders for all the wheel.
    public GameObject wheelShape;
    public WheelCollider[] wheels;
    public WheelCollider[] frontWheels;
    public WheelCollider[] rearWheels;

    // Cosmetics.
    public GameObject steeringWheel, acceleratorPad, breakPad, clutchPad;
    public GameObject ui;
    public GameObject needle;

    // Set values for diffrent things. 
    public float maxSteeringAngle = 10;         // max angle the wheels can turn.
    public float maxSteeringWheelRot = 450;     // max degrees the steeringwheel can turn. (in one direction, + and - )
    public float maxPedalPress = 30;            // max degrees the pedals can be pressed down.
    public float enginePower = 300;             // max power the engine can put out.
    public float slowDownForce = 100.0f;        // the force pushing the car in the opposite direction.
    public float antiRollSpring = 0;            // spring force is used to stableize the car.
    public int   gear = 0;                      // current gear.
    public float velocityZ = 0;                 // current speed in z-axis.
    public float RPM = 1000;                    // current RPM of motor.
    public float test;

    // Declaring an array (ps) to store the four particle systems
    ParticleSystem[] ps;

    public bool includeChildren = true;

    // The distance between gears in units per second
    public float gearDistance = 3.0f;

    void Start()
    {
        Init();         // Needed to run test scen.

        // The particle systems that are children of the car gets inserted into the array ps
        ps = GetComponentsInChildren<ParticleSystem>(includeChildren);
    }

    void FixedUpdate()
    {
        GetInput();
        LogitechGSDK.LogiUpdate();
        RotateSteeringWheel(steeringWheel);
        PressPedals(acceleratorPad, breakPad, clutchPad);
        CalculateTorque();

        // Does something for every wheel collider in the car
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
                    LogitechGSDK.LogiStopBumpyRoadEffect(index);
                    LogitechGSDK.LogiStopDamperForce(index);

                    LogitechGSDK.LogiPlaySlipperyRoadEffect(index, 50);

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
                    LogitechGSDK.LogiStopBumpyRoadEffect(index);
                    LogitechGSDK.LogiStopSlipperyRoadEffect(index);

                    LogitechGSDK.LogiPlayDamperForce(index, 50);

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
                    LogitechGSDK.LogiStopSlipperyRoadEffect(index);
                    LogitechGSDK.LogiStopDamperForce(index);

                    LogitechGSDK.LogiPlayDamperForce(index, 75);
                    LogitechGSDK.LogiPlayBumpyRoadEffect(index, 50);


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

                    slowDownForce = 1000.0f     * wheelMod.resistanceMod;
                }

                // Apply natural slowdown
                rigidbody.AddForce(rigidbody.velocity.normalized * -slowDownForce);
            }
        }

        

        // Call axis stabilizer function using only the first two wheels in each axis
        
        // Front axis
        StabilizeAxis(frontWheels[0], frontWheels[1]);
        // Rear axis
        StabilizeAxis(rearWheels[0], rearWheels[1]);




        // Front wheels
        foreach (var wheel in frontWheels)
        {
            Brake(wheel);
            //Accelerate(wheel);
            Steer(wheel);
            
        }

        // Rear wheels
        foreach (var wheel in rearWheels)
        {
            Brake(wheel);
            Accelerate(wheel);
        }

        // Update positions of wheels last
        foreach (WheelCollider wheel in wheels)
        {
            UpdateWheelPoses(wheel);
        }
    }

    public void Init()
    {
        // MOVE
        {
            // Set the starting position of the car in Showroom.
            this.gameObject.transform.position = new Vector3(24.48f, 0.504f, 28.05f);
            this.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
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

    // Get input from controller
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
    //Rotate the wheels when steering
    void Steer(WheelCollider wheel)
    {
        steeringAngle = horizontalInput * maxSteeringAngle;
        wheel.steerAngle = steeringAngle;
    }

    void CalculateTorque()
    {
        if (ui == null)
        {
            ui = GameObject.FindWithTag("Speedomiter");
            needle = GameObject.FindWithTag("SpeedNeedle");
        }

        // The velocity in positive z direction of the car
        velocityZ = transform.InverseTransformDirection(rigidbody.velocity).z;

        if (velocityZ < 0.1)
        {
            velocityZ = 0;
        }
        int velocityAsInt = (int)((velocityZ * 3) * 3.6);
        string velocityAsString = velocityAsInt.ToString();
        ui.GetComponentInChildren<Text>().text = velocityAsString;



        test = 30 * velocityZ;

        needle.transform.localRotation = Quaternion.Euler(0, 0, 205 - ((velocityZ - 1 - gear * gearDistance) + gearDistance) * 70);

        //ui.GetComponentsInChildren<Image>()[1].transform.rotation.SetEulerAngles(0, 0, 30*velocityZ);
        
        if (gear < 1)
        {
            RPM = 1000 + (((velocityZ * gearDistance) / gear) * 1000) - ((velocityZ * gearDistance) / ((gear - 1) / gearDistance) * 1000);
        }
        else
            RPM = 1000 + ((velocityZ * gearDistance) / (gear) * 1000);

        // Generates a -x^2 curve where velocity is x and y is torque output. The curve is moved in the x-axis depending on the gear and gearDistance
        float motorTorque = -((velocityZ - (Mathf.Abs(gear) - 1) * gearDistance) * (velocityZ - (Mathf.Abs(gear) - 1) * gearDistance) * (enginePower / (gearDistance * gearDistance))) + gasInput * enginePower;

        if (motorTorque < 0 && gear >= 0)
        {
            motorTorque = 0;
        }
        else if (gear < 0)
        {
            // Makes sure reversing gets same negative torque as its positive gear counterpart
            motorTorque = -motorTorque;
            if (motorTorque > 0)
            {
                motorTorque = 0;
            }
        }
        Debug.Log("RPM: " + ((velocityZ - (gear - 1) * gearDistance)) * 1000);

    }

    //Make the car move according to the input
    void Accelerate(WheelCollider wheel)
    {
        // The velocity in positive z direction of the car
        float zVel = transform.InverseTransformDirection(rigidbody.velocity).z;

        //Debug.Log("Gear: " + gear + ", Velocity (km/h):" + ((zVel * 3) * 3.6));

        // Generates a -x^2 curve where velocity is x and y is torque output. The curve is moved in the x-axis depending on the gear and gearDistance
        float motorTorque = -((zVel - (Mathf.Abs(gear) - 1) * gearDistance) * (zVel - (Mathf.Abs(gear) - 1) * gearDistance) * (enginePower / (gearDistance * gearDistance))) + gasInput * enginePower;
        
        if (motorTorque < 0 && gear >= 0 )
        {
            motorTorque = 0;
        }
        else if (gear < 0)
        {
            // Makes sure reversing gets same negative torque as its positive gear counterpart
            motorTorque = -motorTorque;
            if (motorTorque > 0)
            {
                motorTorque = 0;
            }
        }
        
        // Checks if the wheels rotate more than needed in regards to the z velocity. If they are the particle systems starts emitting and if they already are they stop.
        if (wheel.rpm / (60 * wheel.radius * 2 * 3.1415f * 2) > zVel )
        {
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i].Play();
            }
        }
        else if (ps[0].IsAlive() == true)
        {
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i].Stop();
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
        wheel.brakeTorque = brakeInput * enginePower;
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
