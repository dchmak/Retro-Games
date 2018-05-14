/*
* Created by Daniel Mak
*/

using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject ballPrefab;
    public TextMeshProUGUI scoreUI;

    private int score1, score2;

    private void Start() {
        ResetBall();
        ResetScore();
    }

    private void Update() {
        scoreUI.text = score1 + " : " + score2;
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
}