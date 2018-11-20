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

    public void SetCar()
    {
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
        
        car = carPrefabs[chassi.value];
        wheelShape = wheelPrefabs[wheel.value];

        car.SetActive(true);
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

    //REMEMBER TO SCALE DOWN EVERYTHING!!!
    public void Init()
    {
        car.transform.parent = null;
        car.GetComponent<Rigidbody>().isKinematic = false;
        Car carScript = car.GetComponent<Car>();
        carScript.enabled = true;
        carScript.wheelShape = wheelShape;

        wheels = car.GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < wheels.Length; i++)
        {
            GameObject wheelObj = wheels[i].transform.GetChild(0).gameObject;
            DestroyImmediate(wheelObj);
        }

        car.transform.GetChild(0).gameObject.SetActive(true);
        
        carScript.Init();
        
        DontDestroyOnLoad(car);
        SceneManager.LoadScene(uiManager.selectedLevel);
    }
}