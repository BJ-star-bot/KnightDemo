
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dire = new Vector3(0, 0, 0);
    public float force = 15f;
    public Rigidbody rb;
    public bool usegravity = false;
    public float exist_time = 5f;
    public float damage = 100f;
    public GameObject Ownner;
    void Start()
    {

        rb.useGravity = usegravity;
        
        rb.AddForce(dire.normalized * force, ForceMode.VelocityChange);
        Destroy(gameObject, exist_time);
    }
    void OnCollisionEnter(Collision collision)
    {
        hit_target(collision.collider);
        Destroy(gameObject,0.3f);
    }
    void hit_target(Collider target)
    {
        var t = target.GetComponentInParent<IDamageable>();
        if (t != null)
        {
            var dct = new DamageContext
            {
                damage = damage,
                penetration=0,
                damage_source =Ownner
            };

            t.TakeDamage(dct);
        }
        Destroy(gameObject);

    }
 
}
