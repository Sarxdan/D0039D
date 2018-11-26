using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public enum wheelDrive { four, front, rear };
    public enum ControllerType { keyboard, xboxController, ps4Controller, steeringWheel };

    public float  ght;
    public float velocity;

    public WheelFrictionCurve ff;
    public WheelFrictionCurve sf;
    public ControllerType inputType = ControllerType.keyboard;
    public float horizontalInput;
    public float verticalInput;
    public float gasInput = 0.0F;
    public float brakeInput = 0.0F;
    public float clutchInput = 0.0F;
    public float testInput;
    private float steeringAngle;
    
    public GameObject wheelShape;
    public WheelCollider[] wheels;
    public WheelCollider[] frontWheels;
    public WheelCollider[] rearWheels;
    public GameObject steeringWheel, acceleratorPad, breakPad, clutchPad;

    public float maxSteeringAngle = 60;
    public float maxSteeringWheelRot = 450;
    public float maxPedalPress = 30;
    public float enginePower = 500;

    public float antiRollSpring = 50000;

    void Start()
    {
        // Needed to run test scen.
        Init();
    }

    public void Init()
    {
        //inputType = ControllerType.keyboard;
        //Moves the centerofmass
        //GetComponent<Rigidbody>().centerOfMass = GetComponentInChildren<WheelCollider>().transform.position.y
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0.141f, 0.1f);
        
        //Kosmetic object
        steeringWheel = GameObject.FindWithTag("SteeringWheel");
        acceleratorPad = GameObject.FindWithTag("AccelleratorPad");
        breakPad = GameObject.FindWithTag("BreakPad");
        clutchPad = GameObject.FindWithTag("ClutchPad");

        //Get all the Wheel Colliders for the car
        wheels = GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelCollider thisWheel = wheels[i];
            thisWheel.ConfigureVehicleSubsteps(500, 450, 500);

            //Adds the wheel prefab to the car
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
            if (wheel.transform.localPosition.z < 0)
            {
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

    //Get input from controller
    void GetInput()
    {
        if (inputType == ControllerType.keyboard)
        {
            horizontalInput = Input.GetAxis("KeyboardHorizontal");
            verticalInput = Input.GetAxis("KeyboardVertical");
            testInput = Input.GetAxis("KeyboardBack");
            if (Input.GetAxis("KeyboardVertical") > 0)
                gasInput = Input.GetAxis("KeyboardVertical");
            else
                brakeInput = -Input.GetAxis("KeyboardVertical");

        }
        if (inputType == ControllerType.xboxController)
        {
            horizontalInput = Input.GetAxis("XboxHorizontal");
            verticalInput = Input.GetAxis("XboxVertical");
            gasInput = (Input.GetAxis("XboxGas"));
            brakeInput = (Input.GetAxis("XboxBrake"));
            testInput = Input.GetAxis("XboxBack");
        }
        if (inputType == ControllerType.ps4Controller)
        {
            horizontalInput = Input.GetAxis("Ps4Horizontal");
            verticalInput = Input.GetAxis("Ps4Vertical");
            gasInput = (Input.GetAxis("Ps4Gas") + 1) / 2;
            brakeInput = (Input.GetAxis("Ps4Brake") + 1) / 2;
            testInput = Input.GetAxis("Ps4Back");
        }



    }

    void RotateSteeringWheel(GameObject steeringWheel)
    {
        steeringWheel.transform.localEulerAngles = new Vector3(0, -horizontalInput * maxSteeringWheelRot, 0);
    }
    void PressPedals(GameObject accelerator, GameObject breaker, GameObject clutch)
    {
        accelerator.transform.localEulerAngles = new Vector3(-140 + gasInput * maxPedalPress,0,0);
        breaker.transform.localEulerAngles = new Vector3(-140 + brakeInput * maxPedalPress, 0, 0);
        clutch.transform.localEulerAngles = new Vector3(-140 + clutchInput * maxPedalPress, 0, 0);
    }

    //Rotate the wheels when steering
    void Steer(WheelCollider wheel)
    {
        steeringAngle = horizontalInput * maxSteeringAngle;
        wheel.steerAngle = steeringAngle;
    }

    //Make the car move according to the input
    void Accelerate(WheelCollider wheel)
    {
        wheel.motorTorque = gasInput * enginePower;
    }
    void Brake(WheelCollider wheel)
    {
        wheel.brakeTorque = brakeInput * enginePower * 10;
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
    
    void FixedUpdate()
    {
        GetInput();

        RotateSteeringWheel(steeringWheel);
        PressPedals(acceleratorPad, breakPad, clutchPad);

        //Does something for every wheel collider in the car
        foreach (WheelCollider wheel in wheels)
        {
            // checks if the wheel is on a new surface.
            WheelHit hit;
            if (wheel.GetGroundHit(out hit))
            {
                // Modifier value gathered from wheel prefab
                WheelModifier wheelMod = wheel.transform.GetChild(0).GetComponent<WheelModifier>();

                // changes the effectivness of the wheel while on ice!
                if (hit.collider.tag == "ice")
                {

                    ff.asymptoteSlip = 0.4f     * wheelMod.forwardFrictionMod;
                    ff.asymptoteValue = 0.25f   * wheelMod.forwardFrictionMod;
                    ff.extremumSlip = 0.2f      * wheelMod.forwardFrictionMod;
                    ff.extremumValue = 0.5f     * wheelMod.forwardFrictionMod;
                    ff.stiffness = 1            * wheelMod.forwardFrictionMod;
                    sf.asymptoteSlip = 0.1f     * wheelMod.sidewaysFrictionMod;
                    sf.asymptoteValue = 0.375f  * wheelMod.sidewaysFrictionMod;
                    sf.extremumSlip = 0.1f      * wheelMod.sidewaysFrictionMod;
                    sf.extremumValue = 0.5f     * wheelMod.sidewaysFrictionMod;
                    sf.stiffness = 1            * wheelMod.sidewaysFrictionMod;
                    wheel.forwardFriction = ff;
                    wheel.sidewaysFriction = sf;
                }
                // changes the effectivness of the wheel while on tarmac!
                if (hit.collider.tag == "tarmac")
                {
                    ff.asymptoteSlip = 0.4f     * wheelMod.forwardFrictionMod;
                    ff.asymptoteValue = 1.0f    * wheelMod.forwardFrictionMod;
                    ff.extremumSlip = 0.5f      * wheelMod.forwardFrictionMod;
                    ff.extremumValue = 1.2f     * wheelMod.forwardFrictionMod;
                    ff.stiffness = 1            * wheelMod.forwardFrictionMod;
                    sf.asymptoteSlip = 0.6f     * wheelMod.sidewaysFrictionMod;
                    sf.asymptoteValue = 1.2f    * wheelMod.sidewaysFrictionMod;
                    sf.extremumSlip = 0.7f      * wheelMod.sidewaysFrictionMod;
                    sf.extremumValue = 1.5f     * wheelMod.sidewaysFrictionMod;
                    sf.stiffness = 1.0f         * wheelMod.sidewaysFrictionMod;
                    wheel.forwardFriction = ff;
                    wheel.sidewaysFriction = sf;
                }
            }
        }

        // Rear wheels
        

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
            GetComponent<Rigidbody>().AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);
        }

        if (rightGrounded)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
        }
    }
}
