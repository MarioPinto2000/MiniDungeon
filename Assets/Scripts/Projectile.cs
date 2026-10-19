using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask hitLayers; // enemies + ground

    private Rigidbody2D rb;
    private float damage;
    private GameObject owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float direction, float speed, float damage, GameObject owner)
    {
        this.damage = damage;
        this.owner = owner;

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(direction * speed, 0f);

        // Vira o sprite para a direcao do disparo
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return;
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

