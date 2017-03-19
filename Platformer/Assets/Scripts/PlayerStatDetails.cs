using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatDetails : MonoBehaviour {

    int maxHealth;
    float playerCurrentHealth;
    int playerLives;
    int numberOfGems;

    bool hitEnemy = false;
    float enemyDamageCoolDown = 0f;
    float amountOfTimeToCoolDown = 1f;

    CharacterController playerController;
    PlayerUISystem playerUi;
    public int currentCheckPoint = 0;
    public bool playerDead = false;

    public Transform[] spawnPoint;

    // Use this for initialization
    void Start () {
       
        playerUi = Camera.main.GetComponent<PlayerUISystem>();
        playerController = GetComponent<CharacterController>();

        maxHealth = 4;
        playerLives = 3;
        playerCurrentHealth = 0;
        numberOfGems = 0;

        playerUi.DisplayPlayerHealth(playerCurrentHealth, maxHealth);
        playerUi.DisplayLifesText(playerLives);
        playerUi.DisplayGems(numberOfGems);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.N))
        {
            playerUi.ShowPlayerDetails();
        }

        if (enemyDamageCoolDown > 0f)
        {
            enemyDamageCoolDown -= Time.deltaTime;
        }
        else
        {
            hitEnemy = false;
        }

        if (transform.position.y < -10f && !playerDead )
        {
            playerController.StopVelocity();
            KillPlayer();
        }
    }

    void DealDamageToPlayer(float damageAmount)
    {
        if (hitEnemy && enemyDamageCoolDown <= 0f)
        {
            enemyDamageCoolDown = amountOfTimeToCoolDown;
            playerCurrentHealth -= damageAmount;
            playerUi.DisplayPlayerHealth(playerCurrentHealth, maxHealth);

            if (playerCurrentHealth <= 0) {
                KillPlayer();
            }

        } else if (enemyDamageCoolDown > 0f)
        {
            hitEnemy = false;
        }
    }

    void KillPlayer()
    {
        playerDead = true;
        if (playerLives <= 0)
        {
            playerUi.DisplayLifesText(playerLives);
            playerController.playerObject.SetActive(false);
            playerController.enabled = false;
            playerUi.ShowGameOverScreen();
        }
        else
        {
            Vector3 respawnPoint = spawnPoint[currentCheckPoint].gameObject.transform.position;
            playerLives--;
            playerUi.DisplayLifesText(playerLives);
            playerController.playerObject.SetActive(false);
            playerController.enabled = false;
            StartCoroutine(BringPlayerBack(respawnPoint));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            hitEnemy = true;
            DealDamageToPlayer(0.25f);
        }
    }

    IEnumerator BringPlayerBack(Vector3 respawnPos)
    {
        yield return new WaitForSeconds(5f);
        transform.position = respawnPos;
        playerController.enabled = true;
        playerController.playerObject.SetActive(true);
        playerCurrentHealth = maxHealth;
        playerUi.DisplayPlayerHealth(playerCurrentHealth, maxHealth);
        playerController.rb.useGravity = true;
        playerController.lastRot = Vector3.zero;
        playerController.canMoveAfterSpawn = false;
        playerDead = false;
    }
}
