using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private bool gameEnded = false;
    private int maxLives = 3;
    private int lives;
    private Vector2 startPosition;
    private int keysFound = 0;
    private int keysNumber = 3;

    public float jumpForce = 6.0f;
    public float rayLength = 2.0f;
    public LayerMask groundLayer;
    public int score = 0;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        lives = maxLives;
    }
    void Start()
    {

    }
    void Death()
    {
        if(lives > 0)
        {
            lives -= 1;
            transform.position = startPosition;
            Debug.Log($"You have {lives}/3 lives remaining.");
        }
        else
        {
            Debug.Log("You died.");
            StartCoroutine(EndGameAfterDelay(1.0f));
        }
    }

    void KeyFound()
    {
        if(keysFound < keysNumber)
        {
            keysFound++;
            Debug.Log($"You found {keysFound}/{keysNumber} keys");
        }
        else
        {
            Debug.Log("Congratulations! You found all the keys");
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        isWalking = false;

        if (
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.D)
            )
        {
            if (!isFacingRight)
            {
                Flip();
                isFacingRight = true;
            }
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
        }

        if (
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A)
            )
        {
            if(isFacingRight)
            {
                Flip();
                isFacingRight = false;
            }
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
        }

        if (
            Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space)
            )
        {
            Jump();
        }

        Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.red, 1, false);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isWalking", isWalking);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            animator.SetBool("isGrounded", false);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            score += 10;
            Debug.Log("Score:" + score);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Bear"))
        {
            if(keysFound == keysNumber)
            {
                Debug.Log("You've completed your mission. Thank you for the keys.");
                StartCoroutine(EndGameAfterDelay(2.0f));
            }
            else
            {
                Debug.Log("You haven't collected enough keys yet. I can't let you go");
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if(transform.position.y > other.gameObject.transform.position.y)
            {
                score += 50;
                Debug.Log("Killed an enemy.");
            }
            else
            {
                Death();
            }
        }
        else if (other.CompareTag("Key"))
        {
            KeyFound();
            other.gameObject.SetActive(false) ;
        }
        else if(other.CompareTag ("Heal")) {
            if (lives < maxLives)
            {
                lives++;
                other.gameObject.SetActive(false);
                Debug.Log($"Now you have {lives} lives!");
            }
            else
            {
                Debug.Log("You don't need healing right now!");
            }
        }
        else if (other.CompareTag("FallLevel"))
        {
            Death();
        }
    }
    IEnumerator EndGameAfterDelay(float delay)
    {

        
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

