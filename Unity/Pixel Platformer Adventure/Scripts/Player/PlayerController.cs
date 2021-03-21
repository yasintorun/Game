using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [Space]
    [SerializeField] LayerMask WhatIsGround;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallJumpCheck; // duvarlara tırmanabilsin.
    const float groundRadius = .2f;
    public bool isGrounded;
    private bool FacingRight = true;
    public int ExtraJump = 1;

    private Animator anim;
    private Rigidbody2D rb;
    AudioManager audio;

    private void Awake()
    {
        audio = FindObjectOfType<AudioManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
            isGrounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, WhatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    ExtraJump = 1;
                    isGrounded = true;
                }
            }
            anim.SetBool("ground", isGrounded);
            anim.SetFloat("vSpeed", rb.velocity.y);
        
        

    }
    
    public void Move(float move, bool jump)
    {
        anim.SetFloat("speed", Mathf.Abs(move));

        bool isTouchSide = Physics2D.OverlapCircle(wallJumpCheck.position, groundRadius, WhatIsGround);

        if (!isGrounded && move != 0 && isTouchSide)
        {
            ExtraJump = 1;
            anim.SetBool("wallJump", true);
            //anim.SetBool("doubleJump", false); //nedenini bilmedigim şekilde true oluyordu. -> ERROR.
        }
        else
            anim.SetBool("wallJump", false);


        rb.velocity = new Vector2(move * speed, rb.velocity.y);
        if (move > 0 && !FacingRight)
            Flip();
        else if (move < 0 && FacingRight)
            Flip();


        if(jump && (isGrounded || ExtraJump > 0))
        {
            if(audio)
                audio.Play("Player Jump");
            if (!isGrounded)
            {
                anim.SetBool("doubleJump", true);
                ExtraJump--;
            }
            isGrounded = false;
            rb.velocity = Vector2.up * jumpSpeed;
        }
        if (isGrounded && anim.GetBool("doubleJump"))
            anim.SetBool("doubleJump", false);

    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        FacingRight = !FacingRight;
    }

    //for traps
    public void JumpPlayer(float jumpForce = 10)
    {
        ExtraJump = 1;

        anim.SetBool("ground", false);
        rb.velocity = Vector2.up * jumpForce;

    }


    public void DeathPlayer()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
        int dir = YonHesapla();
        //rb.velocity = new Vector2(1 * Random.Range(-1, 2), 2) * Random.Range(4, 7);
        rb.velocity = new Vector2(1 * dir, 2) * Random.Range(4, 7);
        GetComponent<PlayerController>().enabled = false;
    }
    bool die = false;
    public void PlayerDead()
    {
        if(audio)
            audio.Play("Player Death");

        gameObject.tag = "Untagged";
        if (die) return; //1 kere ölünür. :D 
        Camera.main.GetComponent<Animator>().SetTrigger("hit");
        GetComponent<PlayerInput>().enabled = false;
        anim.SetBool("isDead", true);
        anim.SetBool("wallJump", false);
        anim.SetBool("doubleJump", false);
        rb.constraints = RigidbodyConstraints2D.None;
        die = true;
        transform.GetComponentInChildren<BoxCollider2D>().enabled = false;
    }


    void DoubleJumpEvent()
    {
        anim.SetBool("doubleJump", false);
    }



    //öldügünde hangi yone dogru gidecegini hesaplamak için.
    //kameranın solunda ise saga dogru, sagında ise sola dogru hareket et.
    //ortada ise x eksende hareket etme.
    public int YonHesapla()
    {
        if (transform.position.x - Camera.main.transform.position.x < -1) return 1;
        if (transform.position.x - Camera.main.transform.position.x > 1) return -1;
        return 0;
    }
}
