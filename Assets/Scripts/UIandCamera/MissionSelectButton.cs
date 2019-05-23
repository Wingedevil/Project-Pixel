using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSelectButton : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void BasicLoadMission(string scene) {
        SceneManager.LoadScene(scene);
        SceneManager.LoadScene("BasicUI", LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene("Main");
    }
}