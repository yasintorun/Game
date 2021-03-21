using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public float speed = 3f;
    public float startWaitTime;
    private float waitTime = 0f;
    public float delay = 0.1f;
    public Transform[] moveSpots;
    private Transform baseTransform;

    private int index = 0;
    private bool isMove = true;

    public bool isEnemy = false;
    SpriteRenderer sp;
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        baseTransform = transform;
        waitTime = startWaitTime;
    }

    public void Move( bool flip = false)
    {
        if (index >= moveSpots.Length ) index = 0;
        if (EnemyDie && transform.position.y <= -10) Destroy(gameObject);
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[index].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[index].position) < delay)
        {
            isMove = false;
            if (waitTime <= 0)
            {
                index++;
                waitTime = startWaitTime;
                if (index >= moveSpots.Length)
                    index = 0;
                if (flip)
                {
                    Flip();
                }
            }
            else
                waitTime -= Time.deltaTime;
        }
        else
            isMove = true;
    }

    public void SetIsMove(bool move)
    {
        isMove = move;
    }
    public bool GetIsMove()
    {
        return isMove;
    }

    /// <returns>Sag: 1, sol: 2, yukarı:3, aşagı: 4</returns>
    public int WhatIsDir()
    {
        if (baseTransform.position.x < moveSpots[index].position.x)
            return 1;
        if (baseTransform.position.x > moveSpots[index].position.x)
            return 2;
        if (baseTransform.position.y < moveSpots[index].position.y)
            return 3;
        if (baseTransform.position.y > moveSpots[index].position.y)
            return 4;
        else return 0;
    }

    // BURDAN AŞAGISI SADECE ENEMY SCRİPTLER İÇİN.
    void EnemyDead()
    {
        FindObjectOfType<AudioManager>().Play("Enemy Death");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * Random.Range(4, 7);
        enabled = false;
    }

    bool EnemyDie = false;

    public bool Ai_Collision(Collision2D col)
    {
        if (!EnemyDie && col.gameObject.CompareTag("Player"))
        {
            EnemyDie = true; // OnTriggerEnter2D methodundaki player kısmı çalışmaması için.
            col.gameObject.GetComponent<PlayerController>().PlayerDead();
            return true;
        }
        return false;
    }

    public bool Ai_Trigger(Collider2D col, bool firstDie = true)
    {
        if (!EnemyDie && col.gameObject.CompareTag("PlayerFoot"))
        {
            col.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            EnemyDie = true;
            if(firstDie)
                EnemyDead();
            return true;
        }
        return false;
    }

    public bool GetEnemyDie()
    {
        return EnemyDie;
    }
    public void SetEnemyDie(bool e)
    {
        EnemyDie = e;
    }
    public void SetTime(float t)
    {
        waitTime = t;
        startWaitTime = waitTime;
    }


    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void SetIndex(int i)
    {
        index = i;
    }

}
