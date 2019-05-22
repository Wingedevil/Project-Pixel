using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ClickStart() {
        SceneManager.LoadScene("MissionSelect");
    }

    public void ClickExit() {
        Application.Quit(0);
    }

    public void DeleteSaveFile() {
        File.Delete(Application.persistentDataPath + "/save.dat");
    }
}