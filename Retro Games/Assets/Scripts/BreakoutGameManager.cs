/*
* Created by Daniel Mak
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BreakoutGameManager : MonoBehaviour {

    [Range(1, 10)] public int maxLives = 3;

    public GameObject ballPrefab;
    public Vector2 ballSpawn;
    
    public GameObject pauseUI;
    public TextMeshProUGUI livesText;

    public GameObject brickPrefab;
    public Texture2D[] levels;

    private int lives, currentLevel;
    private bool isPausing;
    private float brickHeight, brickWidth, maxBrickCol, maxBrickRow;
    private Vector3 brickSize, bound;
    private List<GameObject> bricks;

    private void Start() {
        lives = maxLives;
        currentLevel = 0;

        brickSize = brickPrefab.GetComponent<SpriteRenderer>().bounds.size;
        brickWidth = brickSize.x;
        brickHeight = brickSize.y;
        bound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxBrickRow = bound.x * 2 / brickWidth;
        maxBrickCol = bound.y / 2 / brickHeight;
        //Debug.Log(maxBrickCol);

        bricks = new List<GameObject>();

        GenerateLevel(levels[currentLevel]);

        ResetBall();
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (isPausing) Unpause();
            else Pause();
        }
    }

    private void LateUpdate() {
        livesText.text = "x " + lives.ToString();

        if (bricks.Count == 0) {
            //Debug.Log("No brick left!");
            currentLevel++;
            if (currentLevel < levels.Length) ResetLevel();
        }

        if (lives <= 0) {
            lives = maxLives;
            currentLevel = 0;
            ResetLevel();
        }
    }

    private void DeleteBalls() {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in balls) {
            Destroy(ball);
        }
    }

    private void SpawnBall() {
        GameObject ball = Instantiate(ballPrefab);

        ball.transform.position = ballSpawn;
    }

    private void GenerateLevel(Texture2D level) {
        GameObject brickHolder = new GameObject();
        brickHolder.name = "Brick Holder";

        for (int y = 0; y < level.height && y < maxBrickCol; y++) {
            for (int x = 0; x < level.width && x < maxBrickRow; x++) {
                Color pixelColor = level.GetPixel(x, y);

                if (pixelColor.a != 0) {
                    Vector2 pos = new Vector2(x * brickWidth - bound.x, y * brickHeight + bound.y / 3);
                    //Debug.Log(pos);
                    GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity);
                    brick.GetComponent<SpriteRenderer>().color = pixelColor;

                    brick.transform.SetParent(brickHolder.transform);
                    bricks.Add(brick);
                }
            }
        }
    }

    private void ResetLevel() {
        foreach (GameObject brick in bricks) {
            Destroy(brick);
        }
        bricks.Clear();
        GenerateLevel(levels[currentLevel]);
    }

    public void ResetBall() {
        //Debug.Log("Deleting balls...");
        DeleteBalls();
        //Debug.Log("Deleted. Spawning ball...");
        Invoke("SpawnBall", 2);
        //Debug.Log("Spawned.");
    }

    public void RemoveBrick(GameObject brickToRemove) {
        bricks.Remove(brickToRemove);
        Destroy(brickToRemove);
    }

    public void TakeDamage() {
        lives--;
    }

    public void Pause() {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        isPausing = true;
    }

    public void Unpause() {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        isPausing = false;
    }

    public void LoadScene(int index) {
        SceneManager.LoadSceneAsync(index);
    }

    public void Quit() {
        Application.Quit();
    }
}