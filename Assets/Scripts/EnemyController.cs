using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    private Animator animator;
    private bool isFacingRight = false;
    private bool isMovingRight = false;
    private float startPositionX;
    public float moveRange = 1.0f;

    void MoveRight()
    {
        if (!isFacingRight)
        {
            Flip();
            isFacingRight = true;
        }
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        if (isFacingRight)
        {
            Flip();
            isFacingRight = false;
        }
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.position.y < other.gameObject.transform.position.y)
            {
                GetComponent<Collider2D>().enabled = false;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (isMovingRight)
            {
                if (this.transform.position.x < startPositionX + moveRange)
                {
                    MoveRight();
                }
                else
                {
                    Flip();
                    MoveLeft();
                }
            }
            else
            {
                if (this.transform.position.x > startPositionX - moveRange)
                {
                    MoveLeft();
                }
                else
                {
                    Flip();
                    MoveRight();
                }
            }
        }

    }
}
