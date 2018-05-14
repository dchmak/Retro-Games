/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour {

    public void LoadScene(int index) {
        SceneManager.LoadSceneAsync(index);
    }

    public void Quit() {
        Application.Quit();
    }
}