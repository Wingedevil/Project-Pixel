using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCamera : MonoBehaviour
{
    public const int PIXELS_PER_UNIT = 128;
    // Start is called before the first frame update
    void Start() {
        this.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize = Screen.height / 2.0f / PIXELS_PER_UNIT;
        this.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectsWithTag("Player")[0].transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
