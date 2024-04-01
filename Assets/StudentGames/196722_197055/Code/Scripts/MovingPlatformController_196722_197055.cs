using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController_196722_197055 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.3f;
    private bool isMovingRight = false;
    private float startPositionX;

    public float moveRange = 0.5f;

    void MoveRight()
    {
        isMovingRight = true;
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        isMovingRight = false;
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    void Awake()
    {
        startPositionX = this.transform.position.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        
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

                    MoveRight();
                }
            }
        }
    }
}
