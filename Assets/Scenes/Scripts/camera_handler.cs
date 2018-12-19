using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_handler : MonoBehaviour {
    public Camera HarryCam, OppoCam;
    public bool camSwitch = false;

    protected virtual void Start()
    {
        Camera[] cameras = Camera.allCameras;
        HarryCam = cameras[0];
        OppoCam = cameras[2];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            camSwitch = !camSwitch;
            HarryCam.gameObject.SetActive(camSwitch);
            OppoCam.gameObject.SetActive(!camSwitch);
        }else if (Input.GetKeyDown(KeyCode.R))
        {
            
        }
    }
}
