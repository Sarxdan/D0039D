using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour {   

    public int index = 0;
    public bool isFinishLine = false;
    public bool isStartLine = false;
    public Sprite goal;

    private GameObject hitbox;

    void Start()
    {
        if (isStartLine)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.25f);
        }
        else if (isFinishLine)
        {
            this.GetComponent<SpriteRenderer>().sprite = goal;
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
        }
        else
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.25f);

        GameObject.FindGameObjectWithTag("Player").GetComponent<CheckpointScript>().init();
    }

}
