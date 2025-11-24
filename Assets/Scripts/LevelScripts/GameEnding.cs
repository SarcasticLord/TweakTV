using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 2.0f;
    public float imageDuration = 2.0f;
    public CanvasGroup canvas;
    float endtimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EndLevel()
    {
        endtimer += Time.fixedDeltaTime;
        canvas.alpha = endtimer /fadeDuration;
        if(endtimer > fadeDuration + imageDuration)
        {
            SceneManager.LoadScene("Buffer");
        }
    }


    public TextMeshProUGUI timerText; // Assign a UI Text element in the Inspector
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}
