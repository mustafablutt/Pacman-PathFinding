using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    [Tooltip("Pacman's movement speed")] public float speed = 5f;

    private Vector3 up = Vector3.zero,
        right = new Vector3(0, 90, 0),
        down = new Vector3(0, 180, 0),
        left = new Vector3(0, 270, 0),
        currentDirection = Vector3.zero;

    private Vector3 nextPos, destination, direction;

    private bool canMove;

    public LayerMask unwalkable;
    //Reset
    private Vector3 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        Reset();
    }

    public void Reset()
    {
        transform.position = initPosition;
        currentDirection = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination,
            speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W))
        {
            nextPos = Vector3.forward;
            currentDirection = up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            nextPos = Vector3.back;
            currentDirection = down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            nextPos = Vector3.left;
            currentDirection = left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            nextPos = Vector3.right;
            currentDirection = right;
        }

        if (Vector3.Distance(destination, transform.position) < 0.00001f)
        {
            transform.localEulerAngles = currentDirection;
            if (Valid())
            {
                destination = transform.position + nextPos;
                direction = nextPos;
            }
        }
    }

    bool Valid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit myHit;

        if (Physics.Raycast(myRay, out myHit, 1f, unwalkable))
        {
            if (myHit.collider.tag == "Wall")
            {
                return false;
            }
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ghost")
        {
            PathFinding pGhost = other.GetComponent<PathFinding>();
            if (pGhost.state == PathFinding.GhostStates.FRIGHTENED)
            {
                pGhost.state = PathFinding.GhostStates.GOT_EATEN;
                GameManager.instance.AddScore(30);
            }
            else if (pGhost.state != PathFinding.GhostStates.FRIGHTENED &&
                     pGhost.state != PathFinding.GhostStates.GOT_EATEN)
            {
                //Lose a life
                Debug.Log("Pacman must lose life");
                GameManager.instance.LoseLife();
            }
        }
    }
}