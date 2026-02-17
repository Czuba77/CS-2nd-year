using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class GeneratedPlatform : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int rad = 10;
    [SerializeField] private int PLATFORMS_NUM = 12;
    [Range(0.01f, 20.0f)] [SerializeField] private float speed =10.0f;
    int POS_NUM = 100;
    int diff;
    float angle;
    int j = 1;


    GameObject[] platforms;
    Vector3[] positions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        diff = POS_NUM / PLATFORMS_NUM;
        POS_NUM = PLATFORMS_NUM * diff;
        angle = 360f / POS_NUM;
        angle *= Mathf.Deg2Rad;
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[POS_NUM];

        for (int i = 0; i < POS_NUM; i++)
        {
            positions[i].x = transform.position.x + Mathf.Cos(angle * i) * rad;
            positions[i].y = transform.position.y + Mathf.Sin(angle * i) * rad;
        }
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i] = Instantiate(platformPrefab, positions[i*diff], Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i].transform.position = Vector2.MoveTowards(platforms[i].transform.position, positions[(diff*i + j) % (POS_NUM)], speed * Time.deltaTime);
            if (platforms[i].transform.position == positions[(diff * i + j) % (POS_NUM)])
            {
                j = (j + 1) % (POS_NUM);
            }
        }
    }
}
