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
    private List<Text> list;

    


	// Add all the text boxes to a list and load all the highscores from the PlayerPrefs to the local arrays
	void Start () {
        list.Add(first);
        list.Add(second);
        list.Add(third);
        list.Add(fourth);
        list.Add(fifth);

         for (int i = 0; i < 5; i++)
        {
            names[i] = PlayerPrefs.GetString(i + "name", "none");
            times[i] = PlayerPrefs.GetInt(i + "time", 0);
           
        }
        

	}
    
    // Update all the text elements to the values stored in the arrays 
    private void updateScoreBoard()
    {
        for (int i = 0; i < 5; i++)
        {
            if(times[i] == 0)
            {
                // Text to show when a highscore is empty
                list[i].text = "-";
            }
            else
            {
                // Show the name and highscore 
                list[i].text = names[i] + " " + times[i].ToString();
            }

        }
        
    }
    // Swap two ints in an array
    private void swapInt(int[] array, int firstIndex, int secondIndex)
    {
        int temp = array[firstIndex];
        array[firstIndex] = array[secondIndex];
        array[secondIndex] = temp;

    }
    // Swap two strings in an array
    private void swapString(string[] array, int firstIndex, int secondIndex)
    {
        string temp = array[firstIndex];
        array[firstIndex] = array[secondIndex];
        array[secondIndex] = temp;

    }
    // Save the highscores to the PlayerPrefs
    private void saveHighscores()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(i + "time", times[i]);
            PlayerPrefs.SetString(i + "name", names[i]);
        }
    }
    // Add a new score the the list and place it in the right place, then update the scoreboard and save the highscores to the PlayerPrefs
    public void addScore(string name, int time)
    {
        names[5] = name;
        times[5] = time;
        for (int i = 5; i > 0; i--)
        {
            if(times[i] < times[i - 1] || times[i - 1] == 0)
            {
                swapInt(times, i, i - 1);
                swapString(names, i, i - 1);
            }
        }


            
        
        updateScoreBoard();
        saveHighscores();
    }
	


}
