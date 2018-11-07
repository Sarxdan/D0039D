using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    enum wheelDrive { four, front, back };
    public float weight;
    public Wheel wheel;
    public float velocity;

	// Use this for initialization
	void Start () {
        //Get wheel script from the first child
        wheel = this.gameObject.transform.GetChild(0).GetComponent<Wheel>();
	}

    void FixedUpdate()
    {
        
    }


    void Update()
    {
        Debug.Log(Input.GetAxis("Horizontal"));
        
    }
}
