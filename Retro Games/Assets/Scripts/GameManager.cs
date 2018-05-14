/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject ballPrefab;
    public TextMeshProUGUI scoreUI;
    public GameObject pauseUI;

    private int score1, score2;
    private bool isPausing;

    private void Start() {
        ResetBall();
        ResetScore();
    }

    private void Update() {
        scoreUI.text = score1 + " : " + score2;

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
        Instantiate(ballPrefab);
    }

    private void ResetScore() {
        score1 = 0;
        score2 = 0;
    }

    public void ResetBall() {
        //Debug.Log("Deleting balls...");
        DeleteBalls();
        //Debug.Log("Deleted. Spawning ball...");
        Invoke("SpawnBall", 2);
        //Debug.Log("Spawned.");
    }

    public void AddScore1() {
        score1++;
    }

    public void AddScore2() {
        score2++;
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