using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime= 3f;
    public float speed= 10f;
<<<<<<< HEAD
=======
    [HideInInspector] public float damage;
>>>>>>> origin/bugra
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right*speed*Time.deltaTime);
    }
}
