using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] public GameObject platformPrefab;
    private static int PLATFORMS_NUM = 7;
    private GameObject[] platforms;
    private Vector3[] positions;
    // Start is called before the first frame update
    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        for(int i = 0; i < PLATFORMS_NUM; i++)
        {
            positions[i] = new Vector3(2.9f+(i*0.5f),-2.2f,0);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
