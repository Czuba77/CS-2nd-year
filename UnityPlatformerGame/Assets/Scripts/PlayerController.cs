using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 1.3f;
    [Range(0.01f, 20.0f)] [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] GameManager manager;
    [SerializeField] AudioClip HeartSound;
    [SerializeField] AudioClip CherrySound;
    [SerializeField] AudioClip GemSound;
    [SerializeField] AudioClip HurtSound;
    [SerializeField] AudioClip JumpSound;
    [SerializeField] AudioClip KillSound;
    [SerializeField] AudioClip WalkingSound;
    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    const float rayLength=1.5f;
    private Animator animator;
    private bool IsWalking = false;
    private bool IsFacingRight = true;
    Vector3 TheScale;
    Vector2 startPosition;
    private bool IsLadder=false;
    private bool IsClimbing=false;
    private float Vertical;
    private AudioSource source;

    const int livesNumber = 3;
    const int keysNumber = 3;


    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        TheScale = this.transform.localScale;
        TheScale.x *= (-1);
        this.transform.localScale = TheScale;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump()
    {
        source.PlayOneShot(JumpSound, AudioListener.volume);
        Debug.Log("jumping");
        if (IsGrounded()) 
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    //[Space(10)]
    // Start is called before the first frame update
    void Start()
    {
    }


    private void FixedUpdate()
    {
        if (IsClimbing)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Vertical * moveSpeed);
        }
        else
            rigidBody.gravityScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState != GameManager.GameState.GAME)
        {
            return;
        }
        Vertical = Input.GetAxis("Vertical");
        IsWalking=false;
        if (Vertical > 0.0 && IsLadder)
        {
            IsClimbing = true;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!IsFacingRight)
                Flip();
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            if(!source.isPlaying && IsGrounded()) source.PlayOneShot(WalkingSound, AudioListener.volume);
            IsWalking = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (IsFacingRight)
                Flip();
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            if (!source.isPlaying && IsGrounded()) source.PlayOneShot(WalkingSound, AudioListener.volume);
            IsWalking = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X))
            Jump();
        //Debug.DrawRay(transform.position,rayLength*Vector3.down,Color.white,1,false);
        animator.SetBool("IsGrounded", IsGrounded());
        animator.SetBool("IsLadder", IsLadder);
        animator.SetBool("IsClimbing", IsClimbing);
        animator.SetBool("IsWalking", IsWalking);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("FallLevel"))
        {
            if (manager.removeLife())
            {
                source.PlayOneShot(HurtSound, AudioListener.volume);
                this.transform.position = startPosition;
            }
            else
            {
                GameManager.instance.setCongratulate("You've lost.");
                GameManager.instance.GameOver();
            }
        }
        if(collision.tag.Equals("Bonus"))
        {
            source.PlayOneShot(CherrySound, AudioListener.volume);
            manager.AddPoints(1);
            collision.gameObject.SetActive(false);
        }
        if (collision.tag.Equals("Ladder"))
        {
            IsLadder = true;
        }
        if (collision.tag.Equals("Enemy"))
        {
            if (this.transform.position.y > collision.transform.position.y)
            {
                manager.AddPoints(10);
                manager.AddEnemyKill();
                source.PlayOneShot(KillSound, AudioListener.volume);
                //Debug.Log("Killed an enemy");
            }
            else
            {
                if (manager.removeLife())
                {
                    source.PlayOneShot(HurtSound, AudioListener.volume);
                    this.transform.position = startPosition;
                }
                else
                {
                    GameManager.instance.setCongratulate("You've lost.");
                    GameManager.instance.GameOver();
                }
            }
        }
        if(collision.tag == "Key")
        {
            manager.AddKeys(collision.name);
            source.PlayOneShot(GemSound, AudioListener.volume);
            collision.gameObject.SetActive(false);
        }
        if (collision.tag == "Hearts")
        {
            manager.AddLives(collision);
            source.PlayOneShot(HeartSound, AudioListener.volume);
            Debug.Log("Znaleziono serce");
        }
        if (collision.tag == "Finish")
        {
            if (manager.GetKeys() > keysNumber-1) {
                //Debug.Log("Zwyciestwo");
                GameManager.instance.AddPoints(GameManager.instance.GetPoints()*9);
                GameManager.instance.AddPoints(GameManager.instance.GetLives()*100);
                GameManager.instance.LevelCompleted();
                
            }
            else
            {
                Debug.Log("Znalaz³es za ma³o kluczy");
            }
        }
        if (collision.tag == "Moving platform")
        {
            this.transform.SetParent(collision.transform);
        }
        if(collision.tag == "Checkpoint")
        {
            startPosition = this.transform.position;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            IsLadder = false;
            IsClimbing = false;
        }
        if (other.tag == "Moving platform")
        {
            this.transform.SetParent(null);
        }
    }
}
