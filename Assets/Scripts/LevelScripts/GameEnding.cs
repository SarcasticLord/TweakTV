using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 2.0f;
    public float imageDuration = 2.0f;
    public CanvasGroup canvas;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EndLevel()
    {
        timer += Time.fixedDeltaTime;
        canvas.alpha = timer /fadeDuration;
        if(timer > fadeDuration + imageDuration)
        {
            SceneManager.LoadScene("Buffer");
        }
    }
}
