using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Fruit: MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("Fruit Collected");
            anim.SetTrigger("dead");
        }
    }
    public void Destroy()
    {
        GameManager.FruitCounter();
        Destroy(gameObject);
    }
}
