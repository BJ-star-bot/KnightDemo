using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageContext dct);
}

public struct DamageContext
{
    public DamageContext(float d = 0, float p = 0,GameObject go = null)
    {
        damage = d;
        penetration = p;
        damage_source = go;
    }
    public float damage;
    public float penetration;
    public GameObject damage_source;
}