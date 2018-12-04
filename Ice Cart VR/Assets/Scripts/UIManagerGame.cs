using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerGame : MonoBehaviour {

    public GameObject pause;
    public GameObject panel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape") && pause.active == false)
        {
            panel.active = true;
            pause.active = true;
            Time.timeScale = 0;
        }
	}

    public void Unpause()
    {
        pause.active = false;
        panel.active = false;
        Time.timeScale = 1;
    }
}
