using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action GoDied;
    public event Action<DamageContext> OnInjured;
    public float full_blood = 100f;
    public float blood;

    void Start()
    {
        blood = full_blood;
    }
    public void TakeDamage(DamageContext dct)

    {
        if (dct.damage_source == null) return;
        blood -= dct.damage;

        Debug.Log("demage from " + dct.damage_source.name);
        if (blood <= 0)
        {
            GoDied.Invoke();
            return;
        }
      
        OnInjured.Invoke(dct);
        
    }

}
