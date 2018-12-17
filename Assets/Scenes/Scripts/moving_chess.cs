using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_chess : MonoBehaviour {

    enum DIR { UP, DOWN, LEFT, RIGHT, LUP, RUP, LDOWN, RDOWN };
    public float smoothTime = 1.0F;
    private Vector3 velocity = Vector3.zero;

    Vector3 targetPosition;

    protected virtual void Start () {
        targetPosition = transform.position;
    }
	
    void move_dir(DIR dir)
    {
        int is_harry = 1;
        if (transform.tag.CompareTo("Harry") != 0)
        {
            is_harry = -1;
        }
        switch (dir)
        {
            case DIR.UP:
                targetPosition = transform.position - is_harry * (new Vector3(0, 0, 5));
                break;
            case DIR.DOWN:
                targetPosition = transform.position - is_harry * (new Vector3(0, 0, -5));
                break;
            case DIR.LEFT:
                targetPosition = transform.position - is_harry * (new Vector3(-5, 0, 0));
                break;
            case DIR.RIGHT:
                targetPosition = transform.position - is_harry * (new Vector3(5, 0, 0));
                break;
            case DIR.LUP:
                targetPosition = transform.position - is_harry * (new Vector3(-5, 0, 5));
                break;
            case DIR.RUP:
                targetPosition = transform.position - is_harry * (new Vector3(5, 0, 5));
                break;
            case DIR.LDOWN:
                targetPosition = transform.position - is_harry * (new Vector3(-5, 0, -5));
                break;
            case DIR.RDOWN:
                targetPosition = transform.position - is_harry * (new Vector3(5, 0, -5));
                break;
        }
    }
    void destroy_chess()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update () {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            return;
        }
        if (Input.GetKey("q")) move_dir(DIR.LUP);
        else if (Input.GetKey("w")) move_dir(DIR.UP);
        else if (Input.GetKey("e")) move_dir(DIR.RUP);
        else if (Input.GetKey("a")) move_dir(DIR.LEFT);
        else if (Input.GetKey("d")) move_dir(DIR.RIGHT);
        else if (Input.GetKey("z")) move_dir(DIR.LDOWN);
        else if (Input.GetKey("x")) move_dir(DIR.DOWN);
        else if (Input.GetKey("c")) move_dir(DIR.RDOWN);
        else if (Input.GetKey(KeyCode.Space)) destroy_chess();
    }
}
