using UnityEngine;

public class up_down : MonoBehaviour
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
        transform.Translate(0, move, 0);

        // Check distance and reverse direction
        if (Vector3.Distance(startPos, transform.position) >= distance)
            movingRight = !movingRight;
    }
}