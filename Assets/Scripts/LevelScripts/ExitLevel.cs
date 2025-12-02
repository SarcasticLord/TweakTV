using EasyPeasyFirstPersonController;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{ 
    private GameObject weapons;
    private GameObject hud;
    private GameObject player;
    private UnityEngine.SceneManagement.Scene currentScene;
    public float cooldownDuration = 3f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        Debug.Log($"Scene: {SceneManager.GetActiveScene()}");
        currentScene = SceneManager.GetActiveScene();
        weapons = GameObject.FindGameObjectWithTag("Hotbar");
        hud = GameObject.FindGameObjectWithTag("HUD");
        player = GameObject.FindGameObjectWithTag("WholePlayer");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WholePlayer"))
        {
            Debug.Log("You beat the level!");
            Singleton.Instance.time = player.GetComponent<GameStats>().elapsedTime;
            if (Singleton.Instance.time < Singleton.Instance.bestTime)
            {
                Singleton.Instance.bestTime = Singleton.Instance.time;
            }
            other.GetComponent<FirstPersonController>().enabled = false;
            weapons.SetActive(false);
            hud.SetActive(false);
        }
    }



    public void StartCooldown()
    {
        StartCoroutine(CooldownAndChangeScene());
        
    }

    private IEnumerator CooldownAndChangeScene()
    {

        yield return new WaitForSeconds(cooldownDuration);
        StartCoroutine(PlayerCooldown()); // Replace with your scene name
    }
    private IEnumerator PlayerCooldown()
    {
        GameStats playerTransition = player.GetComponent<GameStats>();
        yield return new WaitForSeconds(2f);
        playerTransition.EndLevel();
    }


    private void OnTriggerStay(Collider other)
    {
        GameStats playerTransition = player.GetComponent<GameStats>();
        if (other.CompareTag("WholePlayer"))
        {
            if (currentScene.name == "subway")
            {
                StartCooldown();
                other.transform.position = transform.position;
                other.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }
            if (currentScene.name == "AsylumLevel2")
            {
                playerTransition.EndLevel();
                Debug.Log($"Scene: {SceneManager.GetActiveScene().name}");
            }
            if (currentScene.name == "TweakHQ")
            {
                playerTransition.EndLevel();
                Debug.Log($"Scene: {SceneManager.GetActiveScene().name}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
