using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float speed = 10f;

    // Arkadaşının eklediği kısım: Taret ateş ederken bu değeri set edecek
    [HideInInspector] public float damage;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mermiyi sağa doğru (taretin baktığı yöne) ilerletir
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}