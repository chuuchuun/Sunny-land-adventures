using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.05f;
    private bool isFacingRight = true; // Initially facing right
    private int direction = -1; // 1 for right, -1 for left
    public float minX = 1.55f;
    public float maxX = 1.88f;

    void FixedUpdate()
    {
        Vector3 newPosition = transform.position + new Vector3(moveSpeed * direction * Time.fixedDeltaTime, 0.0f, 0.0f);

        if ((direction == 1 && newPosition.x > maxX) || (direction == -1 && newPosition.x < minX))
        {
            direction *= -1;

            Flip();
        }
        else
        {
            transform.position = newPosition;
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
