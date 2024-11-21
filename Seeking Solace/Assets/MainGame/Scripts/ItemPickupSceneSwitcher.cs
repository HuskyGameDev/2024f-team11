using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickupSceneSwitcher : MonoBehaviour
{
    // Flag to check if the item is picked up
    private bool isItemPickedUp = false;

    // Name of the target scene
    public string targetSceneName = "Mini game";

    // Update is called once per frame
    void Update()
    {
        // Check if the item is picked up and T key is pressed
        if (isItemPickedUp && Input.GetKeyDown(KeyCode.T))
        {
            // Load the target scene
            LoadTargetScene();
        }
    }

    // Trigger detection for item pickup
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Item is picked up
            isItemPickedUp = true;

            // Hide or disable the item
            gameObject.SetActive(false);

            Debug.Log("Item picked up! Press 'T' to switch scenes.");
        }
    }

    // Load the target scene
    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("Target scene name is not set!");
        }
    }
}