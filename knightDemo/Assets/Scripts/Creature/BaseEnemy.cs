using UnityEngine;

public class BaseEnemy : MonoBehaviour,HealthProvider
{
    [SerializeField] protected HealthManager healthManager;
    public Rigidbody rb;
    public BaseCreatureConfig creatureConfig;
    protected GameObject target;

    public float Attack = 10f;



    void Awake()
    {
        if (!healthManager) healthManager = GetComponent<HealthManager>();
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

    }

    void OnEnable()//订阅受伤死亡委托
    {
        healthManager.GoDied += DeadMethod;
        healthManager.OnInjured += InjuredAction;
    }
    void OnDisable()
    {
        healthManager.GoDied -= DeadMethod;
        healthManager.OnInjured -= InjuredAction;
    }


    protected virtual void DeadMethod()
    {
        Destroy(gameObject);//可覆写，例如boss不直接消失
    }

    protected virtual void InjuredAction(DamageContext dct)//受伤代码，同样支持覆写
    {
        
    }
    void FixedUpdate()
    {
        
    }

    void RunToTarget()
    {
        if (target != null)
        {
            Vector3 direc = target.transform.position - transform.position;
            direc.y = 0;
            rb.AddForce(direc.normalized * creatureConfig.speed, ForceMode.Acceleration);
        }
    }
    public virtual float getHealth()
    {
        return creatureConfig.health;
    }

    public virtual float getDefense()
    {
        return creatureConfig.defense;
    }
}
