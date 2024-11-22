using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Reference to the item prefab
    public GameObject itemPrefab;

    // Hardcoded spawn position
    public Vector3 spawnPosition = new Vector3(0, 1, 0);

    // Optional: Set spawn rotation
    public Quaternion spawnRotation = Quaternion.identity;

    void Start()
    {
        // Spawn the item at the hardcoded position
        SpawnItem();
    }

    void SpawnItem()
    {
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, spawnPosition, spawnRotation);
        }
        else
        {
            Debug.LogError("Item prefab is not assigned in the inspector.");
        }
    }
}