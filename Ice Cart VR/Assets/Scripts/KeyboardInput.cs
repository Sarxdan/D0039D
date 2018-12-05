using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyboardInput : MonoBehaviour
{
    public enum ControllerType { keyboard, xboxController, ps4Controller, steeringWheel };
    public ControllerType inputType;
    char[] characters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
    public char selectedCharacter;
    private int selectedIndex;
    public Text selected;
    public Text name;
    private string currentTextInBox;
    private string horizontalAxisName;
    private string submitName;
    private string backName;
    private bool isAxisUnUse = false;


    // Use this for initialization
    void Start ()
    {
        selectedIndex = 0;
        selectedCharacter = characters[selectedIndex];
        selected.text = "Character: " + characters[selectedIndex];
        string[] names = Input.GetJoystickNames();
        // Set current inputType to first connected joystick depending on the joysticks name
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Equals("G29 Driving Force Racing Wheel"))
            {
                inputType = ControllerType.steeringWheel;
            }
            else if (names[i].Equals("Xbox One For Windows") || names[i].Equals("Controller (XBOX 360 For Windows)"))
            {
                inputType = ControllerType.xboxController;
            }
            else if (names[i].Equals("Wireless Controller"))
            {
                inputType = ControllerType.ps4Controller;
            }
        }

        if (inputType == ControllerType.ps4Controller)
        {
            horizontalAxisName = "Ps4Vertical";
            submitName = "Ps4Submit";
            backName = "Ps4Back";
        }
        else if(inputType == ControllerType.xboxController)
        {
            horizontalAxisName = "XboxVertical";
            submitName = "XboxSubmit";
            backName = "XboxBack";
        }
        else if (inputType == ControllerType.steeringWheel)
        {
            horizontalAxisName = "SteeringwheelVertical";
            submitName = "SteeringwheelSubmit";
            backName = "SteeringwheelBack";
        }

    }

    void getInput()
    {
            
    }
   
	// Update is called once per frame
	void Update ()
    {
       
        if (Input.GetAxis(horizontalAxisName) != 0)
        {
            if (isAxisUnUse == false)
            {
                if (Input.GetAxis(horizontalAxisName) < 0)
                {
                    if (selectedIndex == 25)
                    {

                    }
                    else
                    {
                        selectedIndex++;
                    }
                    selected.text = "Character: " + characters[selectedIndex];
                }
                else
                {
                    if (selectedIndex == 0)
                    {

                    }
                    else
                    {
                        selectedIndex--;
                    }
                    selected.text = "Character: " + characters[selectedIndex];
                }
                isAxisUnUse = true;
            }
            if(Input.GetAxisRaw(horizontalAxisName) < 0.5 && Input.GetAxisRaw(horizontalAxisName) > -0.5)
            {
                isAxisUnUse = false;
            }
            // Change what character is selected

        }

        if (Input.GetButtonDown(submitName))
        {
            // Add current character to input box
            currentTextInBox = currentTextInBox + characters[selectedIndex];
            name.text = currentTextInBox;
        }
        if (Input.GetButtonDown(backName))
        {
            // Remove previous character from input box
            currentTextInBox = currentTextInBox.Substring(0, currentTextInBox.Length - 1);
            name.text = currentTextInBox;
        }

        

        
        


    }
}
