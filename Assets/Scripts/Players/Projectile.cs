using UnityEngine;

// Straight-line projectile. The prefab needs a Rigidbody2D and a Collider2D with "Is Trigger" enabled.
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f; //Auto-destroy for the missed shots
    [SerializeField] private LayerMask hitLayers; // enemies + ground

    private Rigidbody2D rb;
    private float damage;
    private GameObject owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Called by mage. Damage and direction come from mage
    public void Launch(float direction, float speed, float damage, GameObject owner)
    {
        this.damage = damage;
        this.owner = owner;

        //no gravity and constant velocity in straight line
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(direction * speed, 0f);

        // Flip the sprit the the same direction from where is shooted
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //never collides with shooter
        if (other.gameObject == owner) return;

        //layer mask checkconverts a layer index into a bit. // If that bit is not set in hitLayers, ignore this collider.
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage);
        }

        //Destroyed when valid hit
        Destroy(gameObject);
    }
}

