﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;



public class CheckpointScript : MonoBehaviour {
    public Stopwatch timer;
    // Start at negative one to avoid a special case for the first checkpoint
    public int prevCheckpoint = -1;
    // Use these to spawn at a checkpoint 
    public Vector3 lastCheckpointPosition;
    public Vector3 lastCheckpointRotation;

	// Use this for initialization
	void Start () {
        timer = new Stopwatch();
        timer.Start();
    }
    // Player collides with a trigger colliders (checkpoint)
    void OnTriggerEnter(Collider collision)
    {
        // Check for false collisions
        if(collision != null)
        {
            // Check that the colliders is actully a checkpoint
            if (collision.tag == "Checkpoint")
            {
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
                        timer.Stop();
                    }else if(collision.gameObject.GetComponent<checkpoint>().isStartLine== true)
                    {
                        timer.Start();
                    }
                    // If a player passes the right checkpoint
                    prevCheckpoint = checkpoint;
                    UnityEngine.Debug.Log(timer.Elapsed);
                    lastCheckpointPosition = collision.GetComponent<Transform>().position;
                    lastCheckpointRotation = collision.GetComponent<Transform>().rotation.eulerAngles;
                }
                
            }

        }
    }




}
