using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {
    char[] characters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
    public Text selectedCharacter;
    public InputField name;
    private InputManager inputScript;

	// Use this for initialization
	void Start () {

	}

    void getInput()
    {
            
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
