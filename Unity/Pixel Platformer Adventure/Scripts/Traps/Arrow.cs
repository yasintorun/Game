using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float JumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            GetComponent<Animator>().SetTrigger("hit");
            collision.gameObject.GetComponent<PlayerController>().JumpPlayer(JumpForce);
        }
    }
    public void Destroy2AnimationFinish()
    {
        Destroy(gameObject);
    }
}
