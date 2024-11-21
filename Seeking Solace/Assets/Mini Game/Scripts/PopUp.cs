//using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public GameObject popupUI; // Assign your UI Panel here in the Inspector
    private bool dead = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided
        if (other.CompareTag("Player"))
        {
            // Show the popup
            if (popupUI != null)
            {
                popupUI.SetActive(true);
                dead = true;
            }
        }
    }

    private void Update()
    {
        if (dead && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Mini game");
    }
}

