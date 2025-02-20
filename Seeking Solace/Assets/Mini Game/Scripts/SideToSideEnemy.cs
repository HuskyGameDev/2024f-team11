using UnityEngine;

public class SideToSideEnemy : MonoBehaviour
{
    public float speed = 3f;          // Movement speed
    public float distance = 5f;       // Distance to move from starting point

    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Move enemy side to side
        float move = speed * Time.deltaTime * (movingRight ? 1 : -1);
        transform.Translate(move, 0, 0);

        // Check distance and reverse direction
        if (Vector3.Distance(startPos, transform.position) >= distance)
            movingRight = !movingRight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // "Kill" the player (reload scene or trigger game over)
            Debug.Log("Player killed!");
            Destroy(other.gameObject); // Example: removes player object
        }
    }
}
