using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Points")]
    [SerializeField] public int points;

    private Rigidbody2D rb;
    public float screenWidth;
    public float screenHeight;
    private GameManager _gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        CheckScreenBounds();
    }

    private void HandleMovement() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void HandleJump() 
    {
        if (Input.GetButtonDown("Jump")) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void CheckScreenBounds() 
    {
        Vector2 pos = transform.position;
        if (pos.x < -screenWidth || pos.x > screenWidth || pos.y < -screenHeight || pos.y > screenHeight) 
        {
            _gameManager.HandleScreenBorderCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            _gameManager.HandleBadBall();
        }

        if (collision.CompareTag("Friend")) 
        {
            _gameManager.HandleGoodBall(collision.gameObject);
        }
    }
}
