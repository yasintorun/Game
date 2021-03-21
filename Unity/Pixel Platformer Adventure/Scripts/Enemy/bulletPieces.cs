using UnityEngine;

public class bulletPieces : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        if(transform.parent)
            Destroy(transform.parent.gameObject, 2);
        else
            Destroy(gameObject, 2);
    }
}
