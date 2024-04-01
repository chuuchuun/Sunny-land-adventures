using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController_196722_197055 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.05f;
    private bool isFacingRight = true; // Initially facing right
    private int direction = -1; // 1 for right, -1 for left
    private Animator animator;
    private float startPositionX;
    private int lives = 50;

    public float moveRange = 0.5f;

    void Awake()
    {
        startPositionX = this.transform.position.x;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            Vector3 newPosition = transform.position + new Vector3(moveSpeed * direction * Time.fixedDeltaTime, 0.0f, 0.0f);

            if (direction == -1 && GameManager.instance.foxXPosition > transform.position.x ||
                direction == 1 && GameManager.instance.foxXPosition < transform.position.x)
            {
                direction = -direction;
                Flip();
            }

            if (GameManager.instance.bossFightStarted &&
                newPosition.x > 19.5f &&
                newPosition.x < 21.3f)
            {
                transform.position = newPosition;
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = -theScale.x;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.position.y + 0.3f < other.gameObject.transform.position.y)
            {
                Damage();
            }
        }
    }

    public void Damage()
    {
        lives--;
        Debug.Log($"Bear has {lives} hp");
        if (lives == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager.instance.bossDefeated = true;
        GetComponent<Collider2D>().enabled = false;
        animator.SetBool("isDead", true);
        StartCoroutine(KillOnAnimationEnd());
    }

    IEnumerator KillOnAnimationEnd()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
