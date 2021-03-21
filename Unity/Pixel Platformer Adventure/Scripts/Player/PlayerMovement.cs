/*
    NOT USING
    PLEASE LOOK AT THE `PlayerInput.cs`
*/
using Unity.Profiling;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{

    public float speed = 150f;
    public float jumpSpeed;
    [Space]
    public LayerMask Ground;
    public Transform GroundCheck;
    public float checkRadius;
    public Collider2D isGrounded;
    private bool FacingRight = true;
    private float moveX = 0;
    private int ExtraJump = 1; //ek olarak kaç defa zıplayabilir.

    public float deadSpeed;
    Animator anim;
    Rigidbody2D rb;
    public bool isAndroid;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveInput(float move_x)
    {
        moveX = move_x;
    }
    bool isJumping = false;
    public void AndroidJump()
    {
        isJumping = true;
    }

    void Update()
    {
        if(!isAndroid)
            moveX = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);

        //if (anim.GetBool("isJumping") && jumpClip == anim.GetCurrentAnimatorClipInfo(0)[0].clip)
      //      Debug.Log("Bug"); 
        if (isGrounded && (Mathf.Approximately(rb.velocity.y, 0) || !Mathf.Approximately(rb.velocity.x, 0)))
        {

            anim.SetBool("isJumping", false);
            anim.SetBool("fall", false);
            anim.SetBool("doubleJump", false);
        }

        if ((!isAndroid && Input.GetKeyDown(KeyCode.W)) || (isAndroid && isJumping))
        {
            isJumping = false;
            if(isGrounded)
            {
                anim.SetBool("isJumping", true);
                ExtraJump = 1;
                rb.velocity = Vector2.up * jumpSpeed;
                //isGrounded = false;
            } 
            else if(ExtraJump > 0)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("doubleJump", true);
                ExtraJump--;
                rb.velocity = Vector2.up * jumpSpeed;
            }
        }
        else if (anim.GetBool("isJumping") && rb.velocity.y <= 0)
        {
            anim.SetBool("doubleJump", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("fall", true);
        }

    }

    private void FixedUpdate()
    {
        anim.SetFloat("speed", Mathf.Abs(moveX));
        rb.velocity = new Vector2(moveX * speed * Time.fixedDeltaTime, rb.velocity.y);
        if (moveX > 0 && !FacingRight)
            Flip();
        else if (moveX < 0 && FacingRight)
            Flip();
    }

    void Flip()
    {
        GetComponent<SpriteRenderer>().flipX = FacingRight;
        FacingRight = !FacingRight;
    }

    public void DoubleJumpEvent()
    {
        anim.SetBool("doubleJump", false);
        anim.SetBool("fall", true);
    }

    public void JumpPlayer(float jumpForce = 10, int useGui = 0)
    {
        if (useGui != 0 && ExtraJump <= 0)
        {
            Debug.Log("asd");
            return;
        } else if (useGui == 0)
            ExtraJump = 1;

        anim.SetBool("isJumping", true);
        rb.velocity = Vector2.up * jumpForce;
        
    }

    public void DeathPlayer()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponentInChildren<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
        int d = 1;
        if (Random.Range(0, 2) == 1) d = -1; 
        rb.velocity = new Vector2(1*d, 2) * deadSpeed;
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void PlayerDead()
    {
        //anim.Play("hit");
        anim.SetTrigger("hit");
        
    }
}
