using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public float weight;
    public Wheel wheel;

	// Use this for initialization
	void Start () {
        wheel = this.gameObject.transform.GetComponentInChildren<Wheel>();
	}

    void FixedUpdate()
    {
        
    }


    void Update()
    {
        
    }
}
