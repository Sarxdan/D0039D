using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript_Delete : MonoBehaviour
{
    public GameObject wheelShape;

    // Use this for initialization
    void Start ()
    {
        var ws = Instantiate(wheelShape);
        ws.transform.parent = this.transform;
        ws.transform.position = this.transform.position;
        ws.transform.position = new Vector3(ws.transform.position.x - 0.08f, ws.transform.position.y, ws.transform.position.z);
        ws = Instantiate(wheelShape);
        ws.transform.parent = this.transform;
        ws.transform.position = this.transform.position;
        ws.transform.position = new Vector3(ws.transform.position.x + 0.08f, ws.transform.position.y, ws.transform.position.z);
        ws.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
    }
	
	// Update is called once per frame
	void Update ()
    {
        /// WHAT!! \\\

        this.transform.Rotate(new Vector3(0, 1, 0), Space.Self);
    }
}
