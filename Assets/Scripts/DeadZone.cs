using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeadZone : MonoBehaviour
{
    int score = 0;
    public TMP_Text scoreText;
    public PlatformSpawner platformSpawner;
    public Image gameOverScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            score += 30;
            scoreText.text = "Score: " + score;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Platform"))
        {
            PlayerMovement movementComponent = other.GetComponentInChildren<PlayerMovement>();
            if (movementComponent == null)
            {
                Destroy(other.gameObject);
            }
        }
        else
        {
            platformSpawner.keepSpawning = false;
            gameOverScreen.gameObject.SetActive(true);
            
        }
    }
}
