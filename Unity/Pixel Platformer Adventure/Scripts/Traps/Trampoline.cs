using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float JumpForce;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            anim.SetBool("isJump", true);
            collision.gameObject.GetComponent<PlayerController>().JumpPlayer(JumpForce);
        }
    }

    public void Jump2Idle()
    {
        anim.SetBool("isJump", false);
    }

}
