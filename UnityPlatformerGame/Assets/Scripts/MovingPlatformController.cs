using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 5.0f;
    [Range(0.01f, 20.0f)] [SerializeField] private float moveRange = 10.0f;
    private Animator animator;
    private bool IsMovingRight = true;
    private bool isDead = false;
    float startPositionX;
    Vector3 TheScale;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f, Space.World);
        }
        else
        {
            if (IsMovingRight)
            {
                if (this.transform.position.x < startPositionX + moveRange)
                {
                    MoveRight();
                }
                else
                {
                    IsMovingRight = false;
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
                    IsMovingRight = true;
                    MoveRight();
                }
            }
        }
    }
    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }


    private void Awake()
    {
        startPositionX = transform.localPosition.x;
        animator = GetComponent<Animator>();
    }

}
