using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageContext dct);
}

public struct DamageContext
{
    public DamageContext(float a=0, GameObject b = null)
    {
        basedamage = a;
        damage_source = b;
    }
    public float basedamage;
    public GameObject damage_source;
}