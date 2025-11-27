using System;
using UnityEngine;
using TestGame.Combat;
public class Boss : BaseEnemy
{
    public string Name="x";
    public int boss_score = 3;
    public static Action<string> Ondied;
    
    protected override void DeadMethod()
    {
        base.DeadMethod();
        text_score.Instance.add_score(3);
        Ondied.Invoke(Name);
    }
    protected override void InjuredAction(DamageContext dct)
    {
        base.InjuredAction(dct);
    }

    public void OnTriggerEnter(Collider other)//TODO，boss的简单碰撞伤害结算，现在只是框架，具体的之后再看
    {
        IDamageable target = other.GetComponent<IDamageable>();//获取击中对象上可伤害的类
        if (target == null) return;
        target.TakeDamage(new DamageContext(creatureConfig.attack, creatureConfig.penetration, gameObject));
    }
}
