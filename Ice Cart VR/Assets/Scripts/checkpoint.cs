using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour {   

    public int index = 0;
    public bool isFinishLine = false;
    public bool isStartLine = false;

    private GameObject hitbox;

    void Start()
    {
        if (index == 0)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.25f);
        }
        else
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.25f);
    }

}
