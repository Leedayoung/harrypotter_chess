using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_handler : MonoBehaviour {
    public Camera HarryCam, OppoCam, PersCam;
    public bool camSwitch = false;
    public Camera activeCam;
    public Quaternion target_rotation;
    public PieceManager mPieceManager;
    bool on_process = false;
    bool identifier = false;
    public float smoothTime = 1.0F;
    private float scale = 6.5F;
    private Vector3 velocity = Vector3.zero;
    Vector2Int current_pos_op = new Vector2Int(0, 0);
    Vector2Int current_pos_har = new Vector2Int(0, 7);
    Vector3 targetPosition;
    public int cid_op = 0;
    public int cid_har = 0;
    bool i = true;
    bool j = true;
    public bool pers_id = true;

    protected virtual void Start()
    {
        PersCam.gameObject.SetActive(false);
        HarryCam.gameObject.SetActive(false);
        OppoCam.gameObject.SetActive(false);
        activeCam.gameObject.SetActive(true);
        target_rotation = activeCam.transform.rotation;
    }
    void SetCamPos_oppose()
    {
        float oppo_x_diff_op = scale * (current_pos_op.x - mPieceManager.white_cam.x);
        float oppo_z_diff_op = scale * (current_pos_op.y - mPieceManager.white_cam.y);
        current_pos_op = mPieceManager.white_cam;
        OppoCam.transform.Translate(-oppo_x_diff_op, 0, -oppo_z_diff_op);
    }
    void SetCamPos_harry()
    {
        float oppo_x_diff = scale * (current_pos_har.x - mPieceManager.black_cam.x);
        float oppo_z_diff = scale * (current_pos_har.y - mPieceManager.black_cam.y);
        HarryCam.transform.Translate(oppo_x_diff, 0, oppo_z_diff);
        current_pos_har = mPieceManager.black_cam;
    }

    void rotate_camera()
    {
        Quaternion new_quaternion = Quaternion.Lerp(activeCam.transform.rotation, target_rotation, Time.deltaTime);
        if (new_quaternion == activeCam.transform.rotation)
        {
            activeCam.transform.Rotate(3, -20, 9);
            on_process = false;
        }
        else
        {
            activeCam.transform.rotation = new_quaternion;
        }
    }

    void Update()
    {
        if (mPieceManager.flag1)
        {
            if (cid_op == 0)
            {
                SetCamPos_oppose();
                mPieceManager.flag1 = false;
                cid_op++;
            }
            else
            {
                float oppo_x_diff_op = scale * (current_pos_op.x - mPieceManager.white_cam.x);
                float oppo_z_diff_op = scale * (current_pos_op.y - mPieceManager.white_cam.y);

                if (i)
                {
                    Vector3 targetPosition_temp = OppoCam.gameObject.transform.position;
                    targetPosition_temp.x += oppo_x_diff_op;
                    targetPosition_temp.z += oppo_z_diff_op;
                    targetPosition = targetPosition_temp;
                    i = false;
                }

                if (PieceManager.timer)
                {
                    if ((int)OppoCam.transform.position.x == (int)targetPosition.x && (int)OppoCam.transform.position.z == (int)targetPosition.z)
                    {
                        return;
                    }
                    else
                    {
                        OppoCam.transform.position = Vector3.SmoothDamp(OppoCam.transform.position, targetPosition, ref velocity, smoothTime);
                        return;
                        //Debug.Log(PieceManager.timer);
                        //destroy_chess();
                        //transform.position = targetPosition;
                    }
                }

                current_pos_op = mPieceManager.white_cam;
                i = true;
                mPieceManager.flag1 = false;
            }
        }

        if (mPieceManager.flag2)
        {
            if (cid_har == 0)
            {
                SetCamPos_harry();
                mPieceManager.flag2 = false;
                cid_har++;
            }
            else
            {
                float oppo_x_diff_op = scale * (current_pos_har.x - mPieceManager.black_cam.x);
                float oppo_z_diff_op = scale * (current_pos_har.y - mPieceManager.black_cam.y);

                if (j)
                {
                    Vector3 targetPosition_temp = HarryCam.gameObject.transform.position;
                    targetPosition_temp.x += oppo_x_diff_op;
                    targetPosition_temp.z += oppo_z_diff_op;
                    targetPosition = targetPosition_temp;
                    j = false;
                }

                if (PieceManager.timer)
                {
                    if ((int)HarryCam.transform.position.x == (int)targetPosition.x && (int)HarryCam.transform.position.z == (int)targetPosition.z)
                    {
                        return;
                    }
                    else
                    {
                        HarryCam.transform.position = Vector3.SmoothDamp(HarryCam.transform.position, targetPosition, ref velocity, smoothTime);
                        return;
                        //Debug.Log(PieceManager.timer);
                        //destroy_chess();
                        //transform.position = targetPosition;
                    }
                }

                current_pos_har = mPieceManager.black_cam;
                j = true;
                mPieceManager.flag2 = false;
            }
        }

        if (identifier !=mPieceManager.isBlackTurn && !on_process && !PieceManager.timer)
        {
            pers_id = true;
            PersCam.gameObject.SetActive(false);
            Debug.Log("Changed");
            camSwitch = !camSwitch;
            HarryCam.gameObject.SetActive(camSwitch);
            OppoCam.gameObject.SetActive(!camSwitch);
            if (!camSwitch) activeCam = OppoCam;
            else activeCam = HarryCam;
            target_rotation = activeCam.transform.rotation;
            identifier = mPieceManager.isBlackTurn;

        }
        else if (Input.GetKeyDown(KeyCode.R)&& !on_process)
        {
            target_rotation = Quaternion.identity; ;
            target_rotation.eulerAngles = activeCam.transform.rotation.eulerAngles + new Vector3(0, 30, 0);
            activeCam.transform.Rotate(0, -30, 0);
            on_process = true;
        }
        else if(on_process)
        {
            rotate_camera();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (pers_id)
            {
                HarryCam.gameObject.SetActive(false);
                OppoCam.gameObject.SetActive(false);
                PersCam.gameObject.SetActive(true);
                pers_id = false;
                activeCam = PersCam;
            }
            else
            {
                HarryCam.gameObject.SetActive(camSwitch);
                OppoCam.gameObject.SetActive(!camSwitch);
                PersCam.gameObject.SetActive(false);
                pers_id = true;
                if (!camSwitch) activeCam = OppoCam;
                else activeCam = HarryCam;
            }
        }

        return;
    }

}
