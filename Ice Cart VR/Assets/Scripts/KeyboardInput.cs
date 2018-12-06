using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyboardInput : MonoBehaviour
{
    GameObject car;
    public InputManager.ControllerType inputType;

    char[] characters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
    public char selectedCharacter;
    private int selectedIndex;
    public Text selected;
    public Text name;
    private string currentTextInBox = "";
    public string verticalAxisName;
    private string submitName;
    private string backName;
    private string pauseName;
    public float time;

    private bool canSwitchCharacter = true;
    public float coolDownUntilNextSwitch = 1.0f;


    // Use this for initialization
    void Start ()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        inputType = car.GetComponent<InputManager>().inputType;

        selectedIndex = 0;
        selectedCharacter = characters[selectedIndex];
        selected.text = "Character: " + characters[selectedIndex];

        if (inputType == InputManager.ControllerType.ps4Controller)
        {
            verticalAxisName    = "Ps4Vertical";
            submitName          = "Ps4Submit";
            backName            = "Ps4Back";
            pauseName           = "Ps4Pause";
        }
        else if(inputType == InputManager.ControllerType.xboxController)
        {
            verticalAxisName    = "XboxVertical";
            submitName          = "XboxSubmit";
            backName            = "XboxBack";
            pauseName           = "XboxPause";
        }
        else if (inputType == InputManager.ControllerType.steeringWheel)
        {
            verticalAxisName    = "SteeringwheelVertical";
            submitName          = "SteeringwheelSubmit";
            backName            = "SteeringwheelBack";
            pauseName           = "SteeringwheelPause";
        }
        else if (inputType == InputManager.ControllerType.keyboard)
        {
            verticalAxisName    = "KeyboardVertical";
            submitName          = "KeyboardSubmit";
            backName            = "KeyboardBack";
            pauseName           = "KeyboardPause";
        }
    }

    void getInput()
    {
            
    }
   
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis(verticalAxisName) != 0)
        {
            Debug.Log("activate 1");
            StartCoroutine(SwitchCharacterRoutine(coolDownUntilNextSwitch));
        }
        else if (Input.GetButtonDown(submitName))
        {
            // Add current character to input box
            if (currentTextInBox.Length == 6)
            {
                // limit lenght to 7 characters.
            }
            else
            {
                currentTextInBox = currentTextInBox + characters[selectedIndex];
                name.text = currentTextInBox;
            }
        }
        else if (Input.GetButtonDown(backName))
        {
            // Remove previous character from input box
            if (currentTextInBox.Length != 0)
            {
                currentTextInBox = currentTextInBox.Substring(0, currentTextInBox.Length - 1);
                name.text = currentTextInBox;
            }
        }
        else if (Input.GetButtonDown(pauseName))
        {
            time = car.GetComponent<CheckpointScript>().time;
            GetComponent<HighScore>().addScore(currentTextInBox, time);
        }
        
    }
    // Checkes if the character can be switched based on time since last change.
    IEnumerator SwitchCharacterRoutine(float duration)
    {
        // Checkes if the character can be switched based on time since last change.
        if (canSwitchCharacter)
        {
            Debug.Log("Switch ok: " + duration);
            canSwitchCharacter = false;
            SwitchCharacter();
            yield return new WaitForSeconds(duration);
            canSwitchCharacter = true;
            Debug.Log(canSwitchCharacter);
        }
        else
        {
            Debug.Log("Switch FU: " + duration);
            yield return new WaitForSeconds(0f);
        }
    }
    // Switch the character about to be choosen
    public void SwitchCharacter()
    {
        Debug.Log("Switch being done");
        // move the choosen closer to A
        if (Input.GetAxis(verticalAxisName) < 0)
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
        // moves the choosen closer to Z
        if (Input.GetAxis(verticalAxisName) > 0)
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
    }
}
