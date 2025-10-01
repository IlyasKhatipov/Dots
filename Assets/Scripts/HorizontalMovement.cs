using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    public float speed = 2f; 
    public float distance = 5f; 

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newX = transform.position.x + (movingRight ? speed : -speed) * Time.deltaTime;

        if (newX > startPosition.x + distance)
        {
            newX = startPosition.x + distance;
            movingRight = false;
        }
        else if (newX < startPosition.x - distance)
        {
            newX = startPosition.x - distance;
            movingRight = true;
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}