using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.3f;
    private bool isMovingUp = true;
    private float startPositionY;

    public float moveRange = 2.0f;

    void MoveUp()
    {
        isMovingUp = true;
        transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f, Space.World);
    }

    void MoveDown()
    {
        isMovingUp = false;
        transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f, Space.World);
    }
    void Awake()
    {
        startPositionY = this.transform.position.y;
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
            if (isMovingUp)
            {
                if (this.transform.position.y < startPositionY + moveRange)
                {
                    MoveUp();
                }
                else
                {
                    MoveDown();
                }
            }
            else
            {
                if (this.transform.position.y > startPositionY - moveRange)
                {
                    MoveDown();
                }
                else
                {

                    MoveUp();
                }
            }
        }
    }
}
