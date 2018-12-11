using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour {   

    public int index = 0;
    public bool isFinishLine = false;
    public bool isStartLine = false;

    private GameObject hitbox;

    void Start()
    {
        //this.GetComponent<MeshRenderer>().renderer.enabled = false;
        this.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.5f, 1.0f, 1.00f);
        //this.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
    }

}
