using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected Health health;
    public Rigidbody rb;
    protected GameObject target;
    public float speed = 5f;
    public float Attack = 10f;
    public float invincibleTime = 1.5f;
    protected float invincibleTimer = 0f;
    protected bool invincible = false;
    public void TakeDamage(DamageContext dct)
    {
        if (invincible) return;
        health.TakeDamage(dct);
    }

    void Awake()
    {
        if (!health) health = GetComponent<Health>();
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

    }

    void OnEnable()
    {
        health.GoDied += DeadMethod;
        health.OnInjured += InjuredAction;
    }
    void OnDisable()
    {
        health.GoDied -= DeadMethod;
        health.OnInjured -= InjuredAction;
    }



    protected virtual void DeadMethod()
    {
        Destroy(gameObject);//可覆写，例如boss不直接消失
    }

    protected virtual void InjuredAction(DamageContext dct)//受伤代码，同样支持覆写
    {
        target = dct.damage_source;

    }
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direc = target.transform.position - transform.position;
            direc.y = 0;

            rb.AddForce(direc.normalized * speed, ForceMode.Acceleration);
        }
    }
    void Update()
    {
        IfInvincible();
    }
    void IfInvincible()
    {
                if (invincible)
        {
            invincibleTimer += Time.deltaTime;
        }
        if (invincibleTimer > invincibleTime)
        {
            invincibleTimer = 0;
            invincible = false;
        }
    }
}
