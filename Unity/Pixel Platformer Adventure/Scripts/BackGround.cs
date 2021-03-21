using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed = 0.1f;
    [SerializeField]
    private Sprite[] bgSprite;
    SpriteRenderer sp;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = bgSprite[Random.Range(0, 7)];
    }

    void Update()
    {
        sp.size += Vector2.up * speed*Time.deltaTime;
    }
}
