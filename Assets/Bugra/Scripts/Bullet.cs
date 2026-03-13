using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float speed = 10f;

   
    [HideInInspector] public float damage;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}