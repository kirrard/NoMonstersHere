using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Animator playerAnimator;
    public Rigidbody2D rigid;

    public List<SubPlayer> subPlayers;

    public float speed;
    public float runSpeed;
    public int distance;
    public bool canMove;

    private float currentSpeed;

    private Vector2 vector;

    private float dirX;
    private float dirY;

    private bool isWalking;
    private bool isRunning;

    private Queue<Vector3>[] vectorQueue;

    private Transform playerTf;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        vectorQueue = new Queue<Vector3>[subPlayers.Count];
        playerTf = GetComponent<Transform>();

        for (int i = 0; i < subPlayers.Count; i++)
            vectorQueue[i] = new Queue<Vector3>();

        currentSpeed = speed;
        canMove = true;
    }

    private void Update()
    {
        if (rigid.velocity == Vector2.zero)
            isWalking = false;
        else
            isWalking = true;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * runSpeed;
            isRunning = true;
        }
        else
        {
            currentSpeed = speed;
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            MovePlayer();

        if (isWalking)
        {
            PosEnqueue();
            PosDequeue();
        }

        SetAnim();
    }

    void PosEnqueue()
    {
        for (int i = 0; i < subPlayers.Count; i++)
        {
            vectorQueue[i].Enqueue(playerTf.position);
        }
    }

    void PosDequeue()
    {
        for (int i = subPlayers.Count; i > 0; i--)
        {
            for (int j = vectorQueue[i - 1].Count; j > distance * i; j--)
            {
                subPlayers[i - 1].transform.position = vectorQueue[i - 1].Dequeue();
            }
        }
    }

    void MovePlayer()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        vector.Set(dirX, dirY);

        rigid.velocity = vector * currentSpeed;
    }

    void SetAnim()
    {
        if (vector != Vector2.zero)
        {
            playerAnimator.SetFloat("DirX", vector.x);
            playerAnimator.SetFloat("DirY", vector.y);

            for (int i = 0; i < subPlayers.Count; i++) //each (SubPlayer s in subPlayers)
            {
                subPlayers[i].animator.SetFloat("DirX", vector.x);
                subPlayers[i].animator.SetFloat("DirY", vector.y);
            }
        }

        playerAnimator.SetBool("isWalking", isWalking);

        for (int i = 0; i < subPlayers.Count; i++)
            subPlayers[i].animator.SetBool("isWalking", isWalking);
    }

    /*
    bool IsCollided()
    {
        Debug.DrawRay(transform.position, new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * 0.51f, new Color(0, 1, 0));

        RaycastHit2D raycastHit;
        raycastHit = Physics2D.Raycast(transform.position,
                                        new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 0.51f,
                                        LayerMask.GetMask("CollidingObject"));

        if (raycastHit.transform == null)
        {
            return false;
        }

        return true;
    }*/
}
