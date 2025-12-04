using EasyPeasyFirstPersonController;
using System.Collections;
using UnityEngine;
using static EnemyStates;

public class PlayerHit : MonoBehaviour
{
    public int damageAmount;
    private PlayerHealth health;
    private Transform playerMovement;
    private bool stunned;
    public int stunDuration = 3;
    public AudioSource grunt;
    public GameObject vfx;
    private Vector3 currentPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        health = FindAnyObjectByType<PlayerHealth>();
        playerMovement = GameObject.FindGameObjectWithTag("WholePlayer").GetComponent<Transform>();
    }
    private void Update()
    {
        if (stunned)
        {
            
            playerMovement.position = currentPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") == true)
        {
            Destroy(other);
            Debug.Log("Player Hit!");
            damageAmount = 1;
            Debug.Log($"Damage Taken: {damageAmount}");
            health.PlayerTakeDamage(damageAmount);
        }
        if (other.CompareTag("Train") == true)
        {
            Debug.Log("Player Hit!");
            damageAmount = 5;
            Debug.Log($"Damage Taken: {damageAmount}");
            health.PlayerTakeDamage(damageAmount);
        }
        if (other.CompareTag("BearTrap") == true)
        {
            currentPos = playerMovement.position;
            grunt.Play();
            StartCoroutine(Stunned(stunDuration));
        }
    }

    public IEnumerator Stunned(int duration)
    {
        stunned = true;
        vfx.SetActive(true);
        yield return new WaitForSeconds(duration);
        vfx.SetActive(false);
        stunned = false;
    }
    // Update is called once per frame

}
