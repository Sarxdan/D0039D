﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraBinder : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        if(XRDevice.isPresent)
        {
            this.GetComponent<Canvas>().worldCamera = GameObject.Find("FPP Cam").GetComponent<Camera>();
            this.GetComponent<Canvas>().planeDistance = 0.1f;
        }
        else
        {
            this.GetComponent<Canvas>().worldCamera = GameObject.Find("TPP Cam").GetComponent<Camera>();
            this.GetComponent<Canvas>().planeDistance = 0.1f;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
