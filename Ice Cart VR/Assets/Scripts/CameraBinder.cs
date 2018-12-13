﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBinder : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        this.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        this.GetComponent<Canvas>().planeDistance = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}