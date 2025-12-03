using EasyPeasyFirstPersonController;
using System.Collections;
using UnityEngine;
using static EnemyStates;

public class PlayerHit : MonoBehaviour
{
    public int damageAmount;
    private PlayerHealth health;
    private FirstPersonController playerMovement;
    private bool stunned;
    public int stunDuration = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        health = FindAnyObjectByType<PlayerHealth>();
        playerMovement = GameObject.FindGameObjectWithTag("WholePlayer").GetComponent<FirstPersonController>();
    }
    private void Update()
    {
        if (stunned)
        {
            playerMovement.enabled = false;
        }
        else
        {
            playerMovement.enabled = true;
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
            StartCoroutine(Stunned(stunDuration));
        }
    }

    public IEnumerator Stunned(int duration)
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
    }
    // Update is called once per frame

}
