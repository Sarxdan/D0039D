using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour {


    private float[] times = new float[6];
    private string[] names = new string[6];
    public Text first;
    public Text second;
    public Text third;
    public Text fourth;
    public Text fifth;
    private List<Text> list = new List<Text>();
    private string sceneName;

    


	// Add all the text boxes to a list and load all the highscores from the PlayerPrefs to the local arrays
	void Start ()
    {
        // List is used to show all the scores on the screen
        list.Add(first);
        list.Add(second);
        list.Add(third);
        list.Add(fourth);
        list.Add(fifth);

        // Scene specific scores
        sceneName = SceneManager.GetActiveScene().name;

        for (int i = 0; i < 5; i++)
        {
            names[i] = PlayerPrefs.GetString(i + "name" + sceneName, "none");
            times[i] = PlayerPrefs.GetFloat(i + "time" + sceneName, 0);
        }
        updateScoreBoard();
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
                list[i].text = (i+1) + ": " + names[i] + " " + times[i].ToString();
            }
        }
    }
    // Swap two ints in an array
    private void swapFloat(float[] array, int firstIndex, int secondIndex)
    {
        float temp = array[firstIndex];
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
            PlayerPrefs.SetFloat(i + "time" + sceneName, times[i]);
            PlayerPrefs.SetString(i + "name" + sceneName, names[i]);
        }
    }
    // Add a new score the the list and place it in the right place, then update the scoreboard and save the highscores to the PlayerPrefs
    public void addScore(string name, float time)
    {
        names[5] = name;
        times[5] = time;
        for (int i = 5; i > 0; i--)
        {
            if(times[i] < times[i - 1] || times[i - 1] == 0)
            {
                swapFloat(times, i, i - 1);
                swapString(names, i, i - 1);
            }
        }
        updateScoreBoard();
        saveHighscores();
    }
	public void ResetHighscore()
    {
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey(i + "name" + sceneName))
            {
            PlayerPrefs.DeleteKey(i + "name" + sceneName);
            PlayerPrefs.DeleteKey(i + "time" + sceneName);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            names[i] = PlayerPrefs.GetString(i + "name" + sceneName, "none");
            times[i] = PlayerPrefs.GetFloat(i + "time" + sceneName, 0);
        }
        updateScoreBoard();
    }


}
