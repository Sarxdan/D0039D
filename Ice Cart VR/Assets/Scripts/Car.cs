using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    enum wheelDrive { four, front, back };
    public float weight;
    public Wheel wheel;
    public float velocity;

    public WheelFrictionCurve ff;
    public WheelFrictionCurve sf;
    public float horizontalInput;
    public float verticalInput;
    public float gasInput = 0.0F;
    public float brakeInput = 0.0F;
    private float steeringAngle;
    
    public GameObject wheelShape;
    public WheelCollider[] wheels;

    public float maxSteeringAngle = 60;
    public float enginePower = 500;

    void Start()
    {
        //Get all the Wheel Colliders for the car
        wheels = GetComponentsInChildren<WheelCollider>();
        for(int i = 0; i<wheels.Length; i++)
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
    }

    //Get input from controller
    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        gasInput = (Input.GetAxis("Gas") +  1) / 2;
        brakeInput = (Input.GetAxis("Brake") + 1) / 2;
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
    
    void FixedUpdate()
    {
        GetInput();
        //Does something for every wheel collider in the car
        foreach (WheelCollider wheel in wheels)
        {
            
            UpdateWheelPoses(wheel);
            WheelHit hit;
            if (wheel.GetGroundHit(out hit))
            {

                if (hit.collider.tag == "ice")
                {
                    ff.asymptoteSlip = 0.4f;
                    ff.asymptoteValue = 0.25f;
                    ff.extremumSlip = 0.2f;
                    ff.extremumValue = 0.5f;
                    ff.stiffness = 1;
                    sf.asymptoteSlip = 0.1f;
                    sf.asymptoteValue = 0.375f;
                    sf.extremumSlip = 0.1f;
                    sf.extremumValue = 0.5f;
                    sf.stiffness = 1;
                    wheel.forwardFriction = ff;
                    wheel.sidewaysFriction = sf;
                }
                if (hit.collider.tag == "tarmac")
                {
                    ff.asymptoteSlip = 2.0f;
                    ff.asymptoteValue = 10.0f;
                    ff.extremumSlip = 1.0f;
                    ff.extremumValue = 10;
                    ff.stiffness = 1;
                    sf.asymptoteSlip = 2.0f;
                    sf.asymptoteValue = 10.0f;
                    sf.extremumSlip = 1.0f;
                    sf.extremumValue = 20.0f;
                    sf.stiffness = 1;
                    wheel.forwardFriction = ff;
                    wheel.sidewaysFriction = sf;
                }


            }
            
            //If the wheel is a rear wheel
            if (wheel.transform.localPosition.z < 0)
            {
                Brake(wheel);
                Accelerate(wheel);
            }

            //If the wheel is a front wheel
            if (wheel.transform.localPosition.z > 0)
            {
                Steer(wheel);
                Accelerate(wheel);
            }
        }
    }
}
