using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms_196722_197055 : MonoBehaviour
{
    [SerializeField] public GameObject platformPrefab;
    private static int PLATFORMS_NUM = 14;
    private GameObject[] platforms;
    private Vector3[] positions;
    private float speed = 0.2f;
    private int currentOffset = 1;
    private bool shouldChangeOffset = true;
    // Start is called before the first frame update
    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];

        for(int i = 0; i < PLATFORMS_NUM / 2; i++)
        {
            positions[i] = new Vector3(3.0f+(i*0.5f),-2.0f,0);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }

        for(int i = 0; i < PLATFORMS_NUM / 2; i++)
        {
            positions[PLATFORMS_NUM / 2 + i] = new Vector3(3.0f + (PLATFORMS_NUM / 2 - i - 1) * 0.5f, -2.5f, 0);
            platforms[PLATFORMS_NUM / 2 + i] = Instantiate(platformPrefab, positions[PLATFORMS_NUM / 2 + i], Quaternion.identity);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Approximately(platforms[0].transform.position.x, positions[currentOffset].x) && Mathf.Approximately(platforms[0].transform.position.y, positions[currentOffset].y))
        {
            if (shouldChangeOffset)
            {
                currentOffset = (currentOffset + 1) % PLATFORMS_NUM;
            }

            shouldChangeOffset = false;
        } else
        {
            shouldChangeOffset = true;
        }

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            int nextIndex = (i + currentOffset) % PLATFORMS_NUM;
            platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, positions[nextIndex], speed * Time.deltaTime);
        }
    }
}
