using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_handler : MonoBehaviour {
    public Camera HarryCam, OppoCam;
    public bool camSwitch = false;
    public Camera activeCam;
    public float rotationSpeed = 0.1f;
    public Quaternion target_rotation;
    bool on_process = false;
    protected virtual void Start()
    {

        //Camera[] cameras = Camera.allCameras;
        //HarryCam = cameras[1];
        //OppoCam = cameras[2];
        HarryCam.gameObject.SetActive(false);
        OppoCam.gameObject.SetActive(false);

        activeCam = HarryCam;
        activeCam.gameObject.SetActive(true);
        target_rotation = activeCam.transform.rotation;
    }
    void rotate_camera()
    {
        activeCam.transform.rotation = Quaternion.Lerp(activeCam.transform.rotation, target_rotation, Time.deltaTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !on_process)
        {
            camSwitch = !camSwitch;
            HarryCam.gameObject.SetActive(!camSwitch);
            OppoCam.gameObject.SetActive(camSwitch);
            if (camSwitch) activeCam = OppoCam;
            else activeCam = HarryCam;
            target_rotation = activeCam.transform.rotation;

        }else if (Input.GetKeyDown(KeyCode.R)&& !on_process)
        {
            target_rotation = Quaternion.identity; ;
            target_rotation.eulerAngles = activeCam.transform.rotation.eulerAngles + new Vector3(0, 30, 0);
            activeCam.transform.Rotate(0, -30, 0);
            on_process = true;
        }
        else
        {
            if (on_process && activeCam.transform.rotation == target_rotation)
            {
                activeCam.transform.Rotate(0, -30, 0);
                on_process = false;
                Debug.Log("Process_end");
                
            }else if (on_process)
            {
                rotate_camera();
            }
        }
        return;
    }

}
