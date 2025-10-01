using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    public float speed = 2f; 
    public float height = 3f; 

    private Vector3 startPosition;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = transform.position.y + (movingUp ? speed : -speed) * Time.deltaTime;

        if (newY > startPosition.y + height)
        {
            newY = startPosition.y + height;
            movingUp = false;
        }
        else if (newY < startPosition.y - height)
        {
            newY = startPosition.y - height;
            movingUp = true;
        }

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}