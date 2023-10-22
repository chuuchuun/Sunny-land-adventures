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

    public float jumpForce = 6.0f;
    public float rayLength = 2.0f;
    public LayerMask groundLayer;
    public int score = 0;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

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
            if(score>= 150)
            {
                gameEnded = true;
                StartCoroutine(EndGameAfterDelay(2.0f));
            }
            else
            {
                Debug.Log("You haven't collected enough cherries yet. I can't let you go");
            }
        }
    }
    IEnumerator EndGameAfterDelay(float delay)
    {

        Debug.Log("You've completed your mission. Thank you for the cherries.");
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

