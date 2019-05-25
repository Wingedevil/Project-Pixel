using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public GameObject PauseMenu;

    private bool paused = false;
    // Start is called before the first frame update
    void Start() {
        PauseMenu.SetActive(paused);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause() {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1 ;
        PauseMenu.SetActive(paused);
    }

    public void AbortMission() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MissionSelect");
    }
}
