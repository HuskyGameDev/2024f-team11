using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 4f;           // Enemy movement speed
    public float stoppingDistance = 1f; // How close the enemy gets before stopping
    public float detectionRange = 10f; // Detection range for the enemy

    private Transform player;
    private float initialY; // Store the initial Y position
    private bool playerDetected = false;

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Store the initial Y position
        initialY = transform.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate distance to player (only in X and Z)
            Vector3 playerXZ = new Vector3(player.position.x, initialY, player.position.z);
            float distance = Vector3.Distance(new Vector3(transform.position.x, initialY, transform.position.z), playerXZ);

            // Check if player is within detection range
            if (distance <= detectionRange)
            {
                playerDetected = true;
            }

            // Move towards player if detected and not too close
            if (playerDetected && distance > stoppingDistance)
            {
                Vector3 direction = (playerXZ - transform.position).normalized;

                // Move only in X and Z
                Vector3 movement = new Vector3(direction.x * speed * Time.deltaTime, 0, direction.z * speed * Time.deltaTime);
                transform.position += movement;

                // Face the player (only rotate around Y axis)
                Vector3 lookDirection = playerXZ - transform.position;
                lookDirection.y = 0; // This ensures rotation only around Y axis
                if (lookDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // "Kill" the player and reload the scene
            Debug.Log("Player killed! Reloading scene...");
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Optional: Visualize the detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
