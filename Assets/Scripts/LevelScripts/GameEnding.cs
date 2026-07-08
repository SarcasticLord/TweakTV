using EasyPeasyFirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public int subs;
    public float fadeDuration = 2.0f;
    public float imageDuration = 0.0f;
    public CanvasGroup canvas;
    public TextMeshProUGUI timerText; // Assign a UI Text element in the Inspector
    public float elapsedTime;
    private float endtimer;
    private PlayerHit playerHit;

    public string TimeAsString;

    private void Start()
    {
        playerHit = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHit>();
        elapsedTime = 0f;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EndLevel()
    {
        Singleton.Instance.targetLevel = "EndLevelScene";
        Singleton.Instance.TimeAsString = timerText.text;
        Singleton.Instance.totalElapsedTime = elapsedTime;
        Singleton.Instance.scoreAsString = Singleton.Instance.score.ToString();
        //if (Singleton.Instance.score > Singleton.Instance.highscore)
        //{
        //    Singleton.Instance.highscore = Singleton.Instance.score;
        //    Singleton.Instance.highscoreAsString = Singleton.Instance.highscore.ToString();
        //}
        if (Singleton.Instance.time < Singleton.Instance.bestTime)
        {
            Singleton.Instance.bestTime = Singleton.Instance.time;
        }
        endtimer += Time.fixedDeltaTime;
        canvas.alpha = endtimer /fadeDuration;
        if(endtimer > fadeDuration + imageDuration)
        {
            SceneManager.LoadScene("Buffer");
        }
    }




    void Update()
    {
        elapsedTime += Time.deltaTime;

        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = $"{hours:00}:{minutes:00}:{seconds:00}";

    }
}
