using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMission : MonoBehaviour {
    public string Mission;
    public string BasicUI = "BasicUI";
    public string[] AdditionalUI;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseDown() {
        SceneManager.LoadScene(Mission);
        SceneManager.LoadScene(BasicUI, LoadSceneMode.Additive);
        foreach (string s in AdditionalUI) {
            SceneManager.LoadScene(s, LoadSceneMode.Additive);
        }
    }
}