using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.05f;
    private bool isFacingRight = true; // Initially facing right
    private int direction = -1; // 1 for right, -1 for left
    private float startPositionX;
    public float moveRange = 0.5f;

    void Awake()
    {
        startPositionX = this.transform.position.x;
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            Vector3 newPosition = transform.position + new Vector3(moveSpeed * direction * Time.fixedDeltaTime, 0.0f, 0.0f);

            if ((direction == 1 && newPosition.x > startPositionX + moveRange) || (direction == -1 && newPosition.x < startPositionX - moveRange))
            {
                direction *= -1;

                Flip();
            }
            else
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
}
