using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour {

    StandaloneInputModule InputModule;

    public EventSystem eventSystem;
    public GameObject selectedObject;
    public int selectedLevel;

	// Use this for initialization
	void Start ()
    {

    }

    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {

	}

    public void Quit()
    {
        //Stop playing if called upon in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LevelSelect(int level)
    {
        selectedLevel = level;
    }

    public void StartGame()
    {

    }
}
