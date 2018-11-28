using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HighScore : MonoBehaviour {


    private int[] times = new int[6];
    private string[] names = new string[6];
    public Text first;
    public Text second;
    public Text third;
    public Text fourth;
    public Text fifth;

    


	// Use this for initialization
	void Start () {
        Debug.Log(PlayerPrefs.GetFloat("hello"));
        if (!PlayerPrefs.HasKey("0name"))
        {
            for (int i = 0; i < 5; i++)
            {
               // PlayerPrefs.SetString(i + "name", "none");
                //PlayerPrefs.SetInt(i + "time", 9999);
            }

        }
        for (int i = 0; i < 5; i++)
        {
            //names[i] = PlayerPrefs.GetString(i + "name", "none");
            //times[i] = PlayerPrefs.GetInt(i + "time", 99999);
           
        }
        
       /* first.text = names[0] + " " + times[0].ToString(); 
        first.text = names[1] + " " + times[1].ToString(); 
        first.text = names[2] + " " + times[2].ToString(); 
        first.text = names[3] + " " + times[3].ToString(); 
        first.text = names[4] + " " + times[4].ToString(); */
	}

    public void updateScoreBoard()
    {
        //for (int i = 0; i < length; i++)
        //{

        //}
        first.text = names[0] + " " + times[0].ToString();
        first.text = names[1] + " " + times[1].ToString();
        first.text = names[2] + " " + times[2].ToString();
        first.text = names[3] + " " + times[3].ToString();
        first.text = names[4] + " " + times[4].ToString();

    }

    public void addScore(string name, int time)
    {
        names[5] = name;
        times[5] = time;
        int i, j;
        int N = times.Length;

        for (j = N - 1; j > 0; j--)
        {
            for ( i = 0; i < j; i++)
            {
                if (times[i] > times[i + 1])
                {
                    int tempTime = times[i];
                    string tempName = names[i];
                    times[i] = times[i + 1];
                    names[i] = names[i + 1];
                    times[i + 1] = tempTime;
                    names[i + 1] = tempName;
                            
                }
                            
            }
        }

        names[5] = "none";
        times[5] = 99999;


            
        
        updateScoreBoard();
    }
	
	// Update is called once per frame

}
