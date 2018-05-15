/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BreakoutGameManager : MonoBehaviour {

    public GameObject ballPrefab;
    public Vector2 ballSpawn;

    public GameObject pauseUI;

    public GameObject brickPrefab;
    public Texture2D[] levels;

    private bool isPausing;
    private float brickHeight, brickWidth, maxBrickCol, maxBrickRow;
    private Vector3 brickSize, bound;

    private void Start() {
        brickSize = brickPrefab.GetComponent<SpriteRenderer>().bounds.size;
        brickWidth = brickSize.x;
        brickHeight = brickSize.y;
        bound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxBrickRow = bound.x * 2 / brickWidth;
        maxBrickCol = bound.y / 2 / brickHeight;
        Debug.Log(maxBrickCol);

        GenerateLevel(levels[0]);

        ResetBall();
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Delete)) {
            if (isPausing) Unpause();
            else Pause();
        }
    }

    private void DeleteBalls() {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in balls) {
            Destroy(ball);
        }
    }

    private void SpawnBall() {
        Instantiate(ballPrefab, ballSpawn, Quaternion.identity);
    }

    private void GenerateLevel(Texture2D level) {
        for (int y = 0; y < level.height && y < maxBrickCol; y++) {
            for (int x = 0; x < level.width && x < maxBrickRow; x++) {
                Color pixelColor = level.GetPixel(x, y);

                if (pixelColor.a != 0) {
                    Vector2 pos = new Vector2(x * brickWidth - bound.x, y * brickHeight + bound.y / 3);
                    //Debug.Log(pos);
                    GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity);
                    brick.GetComponent<SpriteRenderer>().color = pixelColor;
                }
            }
        }
    }

    public void ResetBall() {
        //Debug.Log("Deleting balls...");
        DeleteBalls();
        //Debug.Log("Deleted. Spawning ball...");
        Invoke("SpawnBall", 2);
        //Debug.Log("Spawned.");
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