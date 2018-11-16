using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject targetObject;

    private bool buttonSelected;
	 
	void Update () {
        //Makes the eventsystem switch the selected gameobject
		if(buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(targetObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
