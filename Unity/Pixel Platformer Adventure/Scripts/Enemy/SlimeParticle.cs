using UnityEngine;

public class SlimeParticle : MonoBehaviour
{
    public float delay;
    private float timer;

    private void Start()
    {
        timer = 0f + Random.Range(0, delay / 2);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > delay)
        {
            GetComponent<Animator>().SetBool("particle", true);
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFoot"))
        {
            collision.transform.parent.gameObject.GetComponent<PlayerController>().PlayerDead();
            enabled = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
            enabled = false;
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }


}
