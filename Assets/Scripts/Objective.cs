using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    private Scene currentScene;
    private TextMeshProUGUI text;
    private LevelManager levelManager;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        text = GetComponent<TextMeshProUGUI>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void Update()
    {
        if (currentScene.name == "TestRoom")
        {
            text.text = "Do Whatever You Want!";
        }

        else if (currentScene.name == "subway")
        {
            text.text = "Exerminate at least 15 rats!";
            if (levelManager.exitSpawn == true)
            {
                if (text.enabled == false)
                {
                    text.enabled = true;
                }
                text.text = "Get to the Exit Train!";
            }
        }
        else if (currentScene.name == "AsylumLevel2")
        {
            text.text = "Clean Up at least 10 Starsucks Cups!";
            if (levelManager.exitSpawn == true)
            {
                if (text.enabled == false)
                {
                    text.enabled = true;
                }
                text.text = "Find the exit!";
            }
        }
        else if (currentScene.name == "TweakHQ")
        {
            text.text = "Find the Keycard and Escape!";
        }
        else
        {
            text.text = "What objective?";
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            text.enabled = !text.enabled;
        }
    }
}
