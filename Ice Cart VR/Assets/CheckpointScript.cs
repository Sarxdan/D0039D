using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;



public class CheckpointScript : MonoBehaviour {
    public Stopwatch timer;
    public int prevCheckpoint = -1;

	// Use this for initialization
	void Start () {
        timer = new Stopwatch();
        timer.Start();
	}

    void OnTriggerEnter(Collider collision)
    {
        if(collision != null)
        {

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
                        //Player passed finish line
                    }
                    prevCheckpoint = checkpoint;
                    UnityEngine.Debug.Log(timer.Elapsed);
                }
                
            }

        }
    }




}
