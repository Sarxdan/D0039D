using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;



public class CheckpointScript : MonoBehaviour
{
    public Stopwatch timer;
    // Start at negative one to avoid a special case for the first checkpoint
    public int prevCheckpoint = -1;
    // Use these to spawn at a checkpoint 
    public Vector3 lastCheckpointPosition;
    public Quaternion lastCheckpointRotation;
    public bool isTrackComplete = false;
    public float time;
    private InputManager inputScript;
    public Text timeOnScreen;
    public GameObject timeObject;

    // Use this for initialization
    void Start ()
    {
        timer = new Stopwatch();
    }
    //private void Update()
    //{
    //    timeOnScreen = GameObject.Find("TimeOnScreen").GetComponent<Text>();
    //    timeOnScreen.text = CalulateTime();
    //}
    // Player collides with a trigger colliders (checkpoint)
    void OnTriggerEnter(Collider collision)
    {
        // Check for false collisions
        if(collision != null)
        {
            // Check that the colliders is actully a checkpoint
            if (collision.tag == "Checkpoint")
            {
                timeOnScreen = GameObject.Find("TimeOnScreen").GetComponent<Text>();
                int checkpoint = collision.gameObject.GetComponent<checkpoint>().index;

                if(checkpoint > prevCheckpoint + 1)
                {
                    // This means a player has skipped a checkpoint
                }else if( checkpoint <= prevCheckpoint)
                {
                    // Player passed the same checkpoint twice or tried to go backwards
                }
                else
                {
                    if (collision.gameObject.GetComponent<checkpoint>().isFinishLine == true)
                    {
                        UnityEngine.Debug.Log("finish");
                        timer.Stop();
                        isTrackComplete = true;
                        time = timer.ElapsedMilliseconds / 1000f;
                    }
                    else if(collision.gameObject.GetComponent<checkpoint>().isStartLine== true)
                    {
                        //UnityEngine.Debug.Log("start");
                        timer.Start();
                    }
                    // If a player passes the right checkpoint
                    prevCheckpoint = checkpoint;
                    timeOnScreen.text = CalulateTime();
                    //UnityEngine.Debug.Log(timer.Elapsed);
                    lastCheckpointPosition = collision.GetComponent<Transform>().position;
                    lastCheckpointRotation = collision.GetComponent<Transform>().rotation;
                    collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

                }
                
            }

        }
    }

    public string CalulateTime()
    {
        int minutes = 0, seconds = 0;
        time = timer.ElapsedMilliseconds / 1000;
        if (time > 60)
        {
            for (float i = time; i > 60; i -= 60)
            {
                minutes++;
                if ((time - 60) < 60)
                {
                    seconds = (int)(time - 60);
                }
            }
        }
        else
        {
            minutes = 0;
            seconds = (int)time;
        }

        if (minutes < 10)
        {
            if (seconds < 10)
                return "0" + minutes + " : " + "0" + seconds;
            else
                return "0" + minutes + " : " + seconds;
        }
        else
        {
            if (seconds < 10)
                return minutes + " : " + "0" + seconds;
            else
                return minutes + " : " + seconds;
        }
    }



}
