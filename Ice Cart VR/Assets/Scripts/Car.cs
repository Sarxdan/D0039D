using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    enum wheelDrive { four, front, back };
    public float weight;
    public Wheel wheel;
    public float velocity;

    public float horizontalInput;
    public float verticalInput;
    public float gasInput = 0.0F;
    public float brakeInput = 0.0F;
    private float steeringAngle;

    public WheelCollider frontLeftCol, frontRightCol, backLeftCol, backRightCol;
    public Transform frontLeft, frontRight, backLeft, backRight;

    public float maxSteeringAngle = 30;
    public float enginePower = 500;


    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        gasInput = (Input.GetAxis("Gas") + 1) / 2;
        brakeInput = (Input.GetAxis("Brake") + 1) / 2;
    }

    void Steer()
    {
        steeringAngle = horizontalInput * maxSteeringAngle;
        frontLeftCol.steerAngle = steeringAngle;
        frontRightCol.steerAngle = steeringAngle;
    }

    void Accelerate()
    {
        frontLeftCol.motorTorque = gasInput * enginePower;
        frontRightCol.motorTorque = gasInput * enginePower;
    }
    
    void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftCol, frontLeft);
        UpdateWheelPose(frontRightCol, frontRight);
        UpdateWheelPose(backLeftCol, backLeft);
        UpdateWheelPose(backRightCol, backRight);
    }

    void UpdateWheelPose(WheelCollider col, Transform trans)
    {
        Vector3 pos = trans.position;
        Quaternion quat = trans.rotation;

        col.GetWorldPose(out pos, out quat);

        trans.position = pos;
        trans.rotation = quat;
    }

    // Use this for initialization
    void Start () {
        //Get wheel script from the first child
        //wheel = this.gameObject.transform.GetChild(0).GetComponent<Wheel>();
	}

    void FixedUpdate()
    {
        Debug.Log("Test");
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    void Update()
    {
        
    }
}
