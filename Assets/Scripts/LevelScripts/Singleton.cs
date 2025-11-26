using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    public int score;
    public int highscore;
    public float time;
    public float bestTime;


    // Level-based stats
    public Dictionary<int, int> levelScores = new Dictionary<int, int>();
    public Dictionary<int, int> levelHighScores = new Dictionary<int, int>();
    public Dictionary<int, float> levelBestTimes = new Dictionary<int, float>();

    // Global stats
    public float totalElapsedTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void UpdateScore(int level, int score)
    {
        levelScores[level] = score;

        // Update high score if beaten
        if (!levelHighScores.ContainsKey(level) || score > levelHighScores[level])
        {
            levelHighScores[level] = score;
        }
    }

    public void UpdateTime(int level, float time)
    {
        totalElapsedTime += time;

        // Update best time if beaten
        if (!levelBestTimes.ContainsKey(level) || time < levelBestTimes[level])
        {
            levelBestTimes[level] = time;
        }
    }

}
