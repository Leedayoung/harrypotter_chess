using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_chess : MonoBehaviour {

    public Transform target;
    public float smoothTime = 0.1F;
    private Vector3 velocity = Vector3.zero;

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;
    Vector3 targetPosition;
    // Use this for initialization
    protected virtual void Start () {
        //target = transform
        targetPosition = transform.position;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
	}
	
    protected bool Move(int xDir, int yDir,out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }
    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if(hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove && hitComponent != null)
        {
            //onCantMove(hitComponent);
        }


    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while(sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

    }
    bool on_process = false;
    enum DIR {UP,DOWN,LEFT,RIGHT,LUP,RUP,LDOWN,RDOWN} ;
    void harry_move_dir(DIR dir)
    {
        switch (dir)
        {
            case DIR.UP:
                targetPosition = transform.position - (new Vector3(0, 0, 5));
                break;
            case DIR.DOWN:
                targetPosition = transform.position - (new Vector3(0, 0, -5));
                break;
            case DIR.LEFT:
                targetPosition = transform.position - (new Vector3(-5, 0, 0));
                break;
            case DIR.RIGHT:
                targetPosition = transform.position - (new Vector3(5, 0, 0));
                break;
            case DIR.LUP:
                targetPosition = transform.position - (new Vector3(-5, 0, 5));
                break;
            case DIR.RUP:
                targetPosition = transform.position - (new Vector3(5, 0, 5));
                break;
            case DIR.LDOWN:
                targetPosition = transform.position - (new Vector3(-5, 0, -5));
                break;
            case DIR.RDOWN:
                targetPosition = transform.position - (new Vector3(5, 0, -5));
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            return;
        }
        if (Input.GetKey("q")) harry_move_dir(DIR.LUP);
        else if (Input.GetKey("w")) harry_move_dir(DIR.UP);
        else if (Input.GetKey("e")) harry_move_dir(DIR.RUP);
        else if (Input.GetKey("a")) harry_move_dir(DIR.LEFT);
        else if (Input.GetKey("d")) harry_move_dir(DIR.RIGHT);
        else if (Input.GetKey("z")) harry_move_dir(DIR.LDOWN);
        else if (Input.GetKey("x")) harry_move_dir(DIR.DOWN);
        else if (Input.GetKey("c")) harry_move_dir(DIR.RDOWN);
        
    }
}
