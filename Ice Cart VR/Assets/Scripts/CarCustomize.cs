using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CarCustomize : MonoBehaviour {

    public UIManager uiManager;

    public GameObject[] carPrefabs;
    public GameObject[] wheelPrefabs;

    public WheelCollider[] wheels;

    public Dropdown chassi, wheel, drive;
    public GameObject car, wheelShape;

    private void Awake()
    {
        foreach(GameObject car in carPrefabs)
        {
            car.GetComponent<Rigidbody>().isKinematic = true;
            car.GetComponent<Car>().enabled = false;
        }
        SetCar();
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    //Setup the car with correct prefab and wheels
    public void SetCar()
    {
        //If there is already a car on display, delete the former wheels and deactivate the car
        if(car != null)
        {
            wheels = car.GetComponentsInChildren<WheelCollider>();
            for (int i = 0; i < wheels.Length; i++)
            {
                GameObject wheelObj = wheels[i].transform.GetChild(0).gameObject;
                DestroyImmediate(wheelObj);
            }
            car.SetActive(false);
        }
        
        //Take in a car and wheels depending on selection in dropdown-menu
        car = carPrefabs[chassi.value];
        wheelShape = wheelPrefabs[wheel.value];

        car.SetActive(true);
        
        //Add the wheel models and position them according to the wheel collider
        wheels = car.GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelCollider thisWheel = wheels[i];

            //Adds the wheel prefab to the car
            var ws = Instantiate(wheelShape);
            ws.transform.parent = thisWheel.transform;
            
            //####UPDATE WHEEL POSE####

            //Assume that the wheelcolliders first child is the wheel prefab
            Transform shapeTransform = thisWheel.transform.GetChild(0);

            Vector3 pos = shapeTransform.position;
            Quaternion quat = shapeTransform.rotation;

            thisWheel.GetWorldPose(out pos, out quat);

            shapeTransform.position = pos;
            shapeTransform.rotation = quat;
        }
    }
    
    //Makes the car ready to be imported to the level scene
    public void Init()
    {
        car.transform.parent = null;
        car.GetComponent<Rigidbody>().isKinematic = false;
        Car carScript = car.GetComponent<Car>();
        carScript.enabled = true;

        if(drive.value == 0)
            carScript.drive = Car.wheelDrive.front;
        else if(drive.value == 1)
            carScript.drive = Car.wheelDrive.rear;
        else if(drive.value == 2)
            carScript.drive = Car.wheelDrive.four;

        carScript.wheelShape = wheelShape;

        //Clear the wheel models
        wheels = car.GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < wheels.Length; i++)
        {
            GameObject wheelObj = wheels[i].transform.GetChild(0).gameObject;
            DestroyImmediate(wheelObj);
        }

        car.transform.GetChild(0).gameObject.SetActive(true);
        carScript.Init();
        if (uiManager.selectedLevel == 1)
        {
            // Set the starting position of the car in Showroom.
            car.transform.position = new Vector3(24.48f, 0.504f, 28.05f);
            car.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        else if (uiManager.selectedLevel == 2)
        {
            // Set the starting position of the car in Level 2.
            car.transform.position = new Vector3(250f, 0.5f, 250f);
            car.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        else if (uiManager.selectedLevel == 3)
        {
            // Set the starting position of the car in Krig scen 3.
            car.transform.position = new Vector3(156f, 0f, 182f);
            car.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        DontDestroyOnLoad(car);
        SceneManager.LoadScene(uiManager.selectedLevel);
    }
}