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
    private bool isOnWall = false;
    private bool isAttacking = false;

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
        animator.SetBool("isHurt", false);
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
            GameManager.instance.foxXPosition = transform.position.x;
            isWalking = false;
            if (Input.GetKey(KeyCode.Q) && !isOnWall && !isAttacking)
            {
                Attack();
            }
            if (
                Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.D)
                )
            {   
                if (!isFacingRight)
                {
                    Fall();
                    Flip();
                    isFacingRight = true;
                } else if (!isOnWall)
                {
                    transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    isWalking = true;
                }
            }

            if (
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.A)
                )
            {
                if (isFacingRight)
                {
                    Fall();
                    Flip();
                    isFacingRight = false;
                }
                else if (!isOnWall)
                {
                    transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    isWalking = true;
                }
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
            animator.SetBool("isHanging", isOnWall);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump(bool killedEnemy = false)
    {
        if (isOnWall)
        {
            Fall();
            animator.SetBool("isGrounded", false);
            Vector2 jumpVector = ((isFacingRight ? Vector2.left : Vector2.right) + Vector2.up) * 0.7f;
            rigidBody.AddForce(jumpVector * jumpForce, ForceMode2D.Impulse);
            Flip();
            return;
        }

        if (IsGrounded() || killedEnemy)
        {
            animator.SetBool("isGrounded", false);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", isAttacking);
        float forward = isFacingRight ? 0.5f : -0.5f;

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(forward, 2.0f), 0f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                GameObject enemyGameObject = hit.gameObject;
                EnemyController enemyScript = enemyGameObject.GetComponent<EnemyController>();

                if (enemyScript != null)
                {
                    enemyScript.Die();
                    source.PlayOneShot(killSound, AudioListener.volume);
                }
                else
                {
                    Destroy(enemyGameObject);
                    source.PlayOneShot(killSound, AudioListener.volume);
                }
            }
            else if (hit.CompareTag("Bear"))
            {
                GameObject enemyGameObject = hit.gameObject;
                BearController enemyScript = enemyGameObject.GetComponent<BearController>();

                enemyScript.Damage();
                source.PlayOneShot(killSound, AudioListener.volume);
            }
           
        }

        StartCoroutine(StopAttackAnimationonEnd(0.07f));
    }

    private void Hang()
    {
        isOnWall = true;
        rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    private void Fall()
    {
        isOnWall = false;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        if (other.CompareTag("Wall"))
        {
            if (!IsGrounded())
            {
                Hang();
            }
        } else if (other.CompareTag("Bonus"))
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
        else if (other.CompareTag("BossDoor"))
        {
            if (GameManager.instance.GetNumberOfKeysFound() == keysNumber && !GameManager.instance.bossFightStarted)
            {
                other.gameObject.SetActive(false);
                GameManager.instance.bossDoor = other.gameObject;
            }
            else
            {
                Debug.Log("You haven't collected enough keys yet. You can't enter yet");
            }
        }
        else if (other.CompareTag("BossStarter"))
        {
            startPosition = other.gameObject.transform.position;
            GameManager.instance.bossDoor.SetActive(true);
            GameManager.instance.bossFightStarted = true;

        }
        else if (other.CompareTag("Location2"))
        {
            startPosition = other.gameObject.transform.position;

        }
        else if (other.CompareTag("Enemy"))
        {
                if (transform.position.y > other.gameObject.transform.position.y)
                {
                    GameManager.instance.KillEnemy();
                    Jump(true);
                    source.PlayOneShot(killSound, AudioListener.volume);
                    Debug.Log("Killed an enemy.");
                }
                else
                {
                source.PlayOneShot(hurtSound, AudioListener.volume);
                animator.SetBool("isHurt", true);
                StartCoroutine(DeathAfterDelay(1.0f));
            }
        }
        else if (other.CompareTag("FinishDoor"))
        {
            if (GameManager.instance.bossDefeated)
            {
                other.gameObject.SetActive(false);
            }
        }
        else if (other.CompareTag("Bear"))
        {
            if (transform.position.y > other.gameObject.transform.position.y + 0.3f)
            {
                Jump(true);
                source.PlayOneShot(killSound, AudioListener.volume);
            }
            else
            {
                source.PlayOneShot(hurtSound, AudioListener.volume);
                animator.SetBool("isHurt", true);
                StartCoroutine(DeathAfterDelay(1.0f));
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
            source.PlayOneShot(hurtSound, AudioListener.volume);
            animator.SetBool("isHurt", true);
            StartCoroutine(DeathAfterDelay(1.0f));
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
    IEnumerator DeathAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Death();
    }

    IEnumerator StopAttackAnimationonEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }
}

