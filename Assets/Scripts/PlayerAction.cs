using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D rigid;

    public List<SubPlayer> subPlayers;

    public float speed;
    public float runSpeed;
    public int distance;

    private float currentSpeed;

    private Vector2 vector;

    private float dirX;
    private float dirY;

    private Map currentMap;

    private bool isWalking;
    private bool isRunning;

    private Queue<Vector3>[] vectorQueue;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        vectorQueue = new Queue<Vector3>[subPlayers.Count];

        for (int i = 0; i < subPlayers.Count; i++)
            vectorQueue[i] = new Queue<Vector3>();

        currentSpeed = speed;
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
            vectorQueue[i].Enqueue(transform.position);
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

            foreach (SubPlayer s in subPlayers)
            {
                s.animator.SetFloat("DirX", vector.x);
                s.animator.SetFloat("DirY", vector.y);
            }
        }

        playerAnimator.SetBool("isWalking", isWalking);

        foreach (SubPlayer s in subPlayers)
            s.animator.SetBool("isWalking", isWalking);
    }

    /*IEnumerator PosEnqueueCoroutine()
    {
        while (true)
        {
            yield return null;
            WaitForSeconds waitForSeconds;
            waitForSeconds = isRunning ? new WaitForSeconds(followingSpeed / 2) : new WaitForSeconds(followingSpeed);

            Vector3 pos1 = transform.position;
            yield return waitForSeconds;

            Vector3 pos2 = transform.position;
            yield return waitForSeconds;

            Vector3 pos3 = transform.position;
            yield return waitForSeconds;

            Debug.Log(pos1);
            Debug.Log(pos2);
            Debug.Log(pos3);

            vectorQueue.Enqueue(pos1);
            vectorQueue.Enqueue(pos2);
            vectorQueue.Enqueue(pos3);
        }
    }*/

    /*
    void MoveSubPlayer()
    {
        if (isWalking)
        {
            int index = 0;

            foreach (SubPlayer s in subPlayers)
            {
                Vector3 position;

                if (dirX == 0)
                {
                    position = new Vector3(0, -vector.y, transform.position.z);
                    position.y += (vector.y > 0) ? -index : index;
                    position.y *= 0.5f;
                }
                else if (dirY == 0)
                {
                    position = new Vector3(-vector.x, 0, transform.position.z);
                    position.x += (vector.x > 0) ? -index : index;
                    position.x *= 0.5f;
                }
                else
                {
                    position = new Vector3(-vector.x, -vector.y, transform.position.z);
                    position.x += (vector.x > 0) ? -index : index;
                    position.y += (vector.y > 0) ? -index : index;
                    position.x *= 0.5f;
                    position.y *= 0.5f;
                }

                index++;

                s.transform.position = Vector3.Slerp(s.transform.position, transform.position + position, followingSpeed * Time.deltaTime);
                //s.transform.position = transform.position + position;

                if (vector == Vector2.zero)
                {
                    s.animator.SetBool("isWalking", false);
                }
                else
                {
                    s.animator.SetFloat("DirX", vector.x);
                    s.animator.SetFloat("DirY", vector.y);
                    s.animator.SetBool("isWalking", true);
                }
            }
        }
    }
    */

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
