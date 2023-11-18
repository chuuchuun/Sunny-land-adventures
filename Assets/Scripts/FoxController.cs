using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] public AudioClip bonusSound;
    [SerializeField] public AudioClip hurtSound;
    [SerializeField] public AudioClip killSound;
    [SerializeField] public AudioClip healSound;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private int maxLives = 3;
    private Vector2 startPosition;
    private int keysNumber = 3;
    private AudioSource source;
    public float jumpForce = 6.0f;
    public float rayLength = 2.0f;
    public LayerMask groundLayer;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        source = GetComponent<AudioSource>();
    }
    void Start()
    {

    }
    void Death()
    {
        if(GameManager.instance.GetLives() > 1)
        {
            transform.position = startPosition;
            GameManager.instance.Death();
        }
        else
        {
            Debug.Log("You died.");
            StartCoroutine(EndGameAfterDelay(1.0f));
        }
    }


   
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
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
                if (isFacingRight)
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
            GameManager.instance.AddPoints(10);
            other.gameObject.SetActive(false);
            source.PlayOneShot(bonusSound, AudioListener.volume);
        }
        else if (other.CompareTag("Exit"))
        {
            if(GameManager.instance.GetNumberOfKeysFound() == keysNumber)
            {
                GameManager.instance.AddPoints(100 * GameManager.instance.GetLives());
                Debug.Log("You've completed your mission. Thank you for the keys.");
                GameManager.instance.LevelCompleted();

            }
            else
            {
                Debug.Log("You haven't collected enough keys yet. I can't let you go");
            }
        }
        else if (other.CompareTag("Enemy"))
        {
                if (transform.position.y > other.gameObject.transform.position.y)
                {
                    GameManager.instance.KillEnemy();
                    source.PlayOneShot(killSound, AudioListener.volume);
                    Debug.Log("Killed an enemy.");
                }
                else
                {
                source.PlayOneShot(hurtSound, AudioListener.volume);
                Death();
                }
        }
        else if (other.CompareTag("Key"))
        {
            GameManager.instance.AddKeys();
            other.gameObject.SetActive(false) ;
        }
        else if(other.CompareTag ("Heal")) {
            if (GameManager.instance.GetLives() < maxLives)
            {
                GameManager.instance.Heal();
                other.gameObject.SetActive(false);
                source.PlayOneShot(healSound, AudioListener.volume);
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
        else if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.SetParent(null);
    }
    IEnumerator EndGameAfterDelay(float delay)
    {

        
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

