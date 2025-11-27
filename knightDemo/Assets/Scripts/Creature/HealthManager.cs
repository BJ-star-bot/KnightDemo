using System;
using UnityEngine;

public class HealthManager : MonoBehaviour,IDamageable//通用生命值代码，所有有生命值并且可受伤的生物都可以挂这个脚本
{
    public event Action GoDied;
    public event Action<DamageContext> OnInjured;

    
    public HealthProvider Hp;
    [SerializeField,HideInInspector]
    public float currentHealth;//这个脚本储存对象的真实生命值，生命条脚本从这里取生命值变动
    
    
    void Start()
    {
        Hp = gameObject.GetComponent<HealthProvider>();
        if (Hp == null) return;
        currentHealth = Hp.getHealth();
        
    }
    public void TakeDamage(DamageContext damageContext)//实现IDamageable暴露了的受伤的方法，其他物体从这个方法输入伤害值与穿透率

    {
        if (Hp == null) return;
        if (damageContext.damageSource == null) return;
        float realDamage = damageContext.damage - Hp.getDefense() * (1 - damageContext.penetration);
        if (realDamage < damageContext.damage * 0.1f) realDamage = damageContext.damage * 0.1f;//每次伤害保底造成基础伤害的10%
        currentHealth -= realDamage;

        Debug.Log("demage from " + damageContext.damageSource.name);
        if (currentHealth <= 0)
        {
            GoDied?.Invoke();
            return;
        }
        OnInjured?.Invoke(damageContext);
        
    }

}
