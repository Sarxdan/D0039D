using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceTest : MonoBehaviour {

    // Use this for initialization
    private Rigidbody rb;
    private float resistance = 0.5f;
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(1, 0, 0) * 1000);
    }
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(new Vector3(-rb.velocity.x * resistance, - rb.velocity.y * resistance, - rb.velocity.z * resistance));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground");
        {
            //resistance += 10;
           // rb.AddForce(new Vector3(-rb.velocity.x * resistance, -rb.velocity.y * resistance, -rb.velocity.z * resistance));
            Surface script = collision.gameObject.GetComponent<Surface>();
            resistance = script.resistance;
        }
       
    }
}
