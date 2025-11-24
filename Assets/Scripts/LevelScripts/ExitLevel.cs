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
    public float cooldownDuration = 10f;



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
        //UnityEngine.SceneManagement.Scene current = SceneManager.GetActiveScene();
        //SceneManager.UnloadSceneAsync(current);
        SceneManager.LoadScene("EndLevelScene"); // Replace with your scene name
        //SceneManager.UnloadScene()
    }

    
    private void OnTriggerStay(Collider other)
    {
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
                Destroy(player);
                SceneManager.LoadScene("Buffer");
                Debug.Log($"Scene: {SceneManager.GetActiveScene().name}");

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
