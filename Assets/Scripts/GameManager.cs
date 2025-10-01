using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GoodBallPrefab;
    public GameObject[] BadBallPrefabs; 
    public Button RestartButton;
    public TextMeshProUGUI ScoresText;
    public TextMeshProUGUI HighestScoresText;
    public TextMeshProUGUI YOUDIED;
    public GameObject tutorPanel;
    public float screenBorderOffset = 0.3f;

    private Camera mainCamera;
    private PlayerController _player;
    public float screenWidth;
    public float screenHeight;

    private int highestScore;
    private const string HighScoreKey = "HighScore";
    private const string FirstLaunchKey = "FirstLaunch";

    private void Awake()
    {
        StopGame();
        mainCamera = Camera.main;
        CalculateScreenBounds();
        _player = FindObjectOfType<PlayerController>();
        _player.screenHeight = screenHeight;
        _player.screenWidth = screenWidth;
        RestartButton.gameObject.SetActive(false);
        YOUDIED.gameObject.SetActive(false);

        highestScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UpdateHighestScoreText();

        ScoresText.text = $"Scores: {_player.points}";
        Time.timeScale = 1f;
        SpawnBadBall();
        SpawnGoodBall();
    }

    private void CalculateScreenBounds()
    {
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        screenWidth = screenBounds.x - screenBorderOffset;
        screenHeight = screenBounds.y - screenBorderOffset;
    }

    private bool IsPositionValid(Vector2 position, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        return colliders.Length == 0;
    }

    public Vector2 CalculateValidPosition()
    {
        float radius = 0.09f;
        int maxAttempts = 50;
        Vector2 spawnPosition;
        bool positionIsValid = false;
        int attempts = 0;

        do
        {
            spawnPosition = new Vector2(
                Random.Range(-screenWidth, screenWidth),
                Random.Range(-screenHeight, screenHeight));

            positionIsValid = IsPositionValid(spawnPosition, radius);
            attempts++;
        }
        while (!positionIsValid && attempts < maxAttempts);

        return spawnPosition;
    }

    public void SpawnGoodBall()
    {
        Vector2 spawnPosition = CalculateValidPosition();
        Instantiate(GoodBallPrefab, spawnPosition, Quaternion.identity);
    }

    public void SpawnBadBall()
    {
        if (BadBallPrefabs.Length == 0) return;

        Vector2 spawnPosition = CalculateValidPosition();
        GameObject randomBadBall = BadBallPrefabs[Random.Range(0, BadBallPrefabs.Length)];
        Instantiate(randomBadBall, spawnPosition, Quaternion.identity);
    }

    public void HandleGoodBall(GameObject GoodBall)
    {
        _player.points += 1;
        ScoresText.text = $"Scores: {_player.points}";

        if (_player.points > highestScore)
        {
            highestScore = _player.points;
            PlayerPrefs.SetInt(HighScoreKey, highestScore);
            UpdateHighestScoreText();
        }

        Destroy(GoodBall);
        SpawnGoodBall();
        SpawnBadBall();
    }

    public void HandleBadBall()
    {
        Die();
    }

    public void Die()
    {
        if (_player.points > highestScore)
        {
            highestScore = _player.points;
            PlayerPrefs.SetInt(HighScoreKey, highestScore);
            UpdateHighestScoreText();
        }

        Time.timeScale = 0f;
        RestartButton.gameObject.SetActive(true);
        YOUDIED.gameObject.SetActive(true);
    }

    public void HandleScreenBorderCollision()
    {
        Die();
    }

    public void Restart()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current);
    }

    private void UpdateHighestScoreText()
    {
        HighestScoresText.text = $"Highest: {highestScore}";
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        Physics2D.autoSimulation = false;
        tutorPanel.SetActive(true);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        Physics2D.autoSimulation = true;
        tutorPanel.SetActive(false);
    }
}