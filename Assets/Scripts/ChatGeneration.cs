using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class ChatGeneration : MonoBehaviour
{
    private string[] usernames = File.ReadAllLines("Assets/Scripts/usernames.txt");
    private string[] chatmessages = File.ReadAllLines("Assets/Scripts/chatmessages.txt");
    public TextMeshProUGUI chat;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateChat());
    }
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    void newChatMessage()
    {
        int randomName = UnityEngine.Random.Range(0, usernames.Length);
        int randomMesssage = UnityEngine.Random.Range(0, chatmessages.Length);
        chat.text = usernames[randomName] + ": " + chatmessages[randomMesssage] + "\n";
    }
    IEnumerator UpdateChat()
    {
        while (true)
        { 
            newChatMessage();
            yield return new WaitForSeconds(1f);
        }
    }

}
