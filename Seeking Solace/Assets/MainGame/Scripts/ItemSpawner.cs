using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Reference to the item prefab
    public GameObject itemPrefab;

    // Hardcoded spawn position
    public Vector3 spawnPosition = new Vector3(0, 1, 0);

    // Spawn position array
    public GameObject[] spawnLocations;

    // Optional: Set spawn rotation
    public Quaternion spawnRotation = Quaternion.identity;

    // Spawn location tag
    public string spawnTag;

    void Start()
    {
        // Spawn the item at a randomly selected spawn location
        spawnLocations = GameObject.FindGameObjectsWithTag(spawnTag);
        if(spawnLocations.Length < 1) {
            Debug.LogError("No spawn locations found.");
            return;
        }
        else{
            spawnPosition = spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position;
        }
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