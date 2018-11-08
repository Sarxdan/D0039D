using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour {

    // Use this for initialization
    //These variables controls how the road surface feels to the player
    public WheelFrictionCurve forwardFriction;
    public WheelFrictionCurve sidewaysFriction;
    [Range(0,2)]
    public float asSlip = 1.5f;
    [Range(0, 10000)]
    public float asValue = 8000;
    [Range(0, 1)]
    public float exSlip = 0.8f;
    [Range(0, 20000)]
    public float exValue = 18000;


    
    public float resistance = 0.1f;
	void Start () {
        
        forwardFriction.asymptoteSlip = asSlip;
        forwardFriction.asymptoteValue = asValue;
        forwardFriction.extremumValue = exValue;
        forwardFriction.extremumSlip = exSlip;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
