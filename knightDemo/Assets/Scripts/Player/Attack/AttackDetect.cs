using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackDetect : MonoBehaviour//武器击中检测
{
    public Collider col;
    private string actionName;
    private bool hitAble;
    private HashSet<IDamageable> hitTarget;//用hashset去重更快
    void Awake()
    {
        if (col == null) col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false;
        hitTarget = new HashSet<IDamageable>();

    }
    public void enableHitBox(string id)//在动画中设置事件来触发，不在代码中调用
    {
        col.enabled = true;
        hitTarget.Clear();
        hitAble = true;
        actionName = id;//获取当前招式名称，根据此来设置伤害倍率，耐力消耗等
    }
    public void disableHitBox()
    {
        col.enabled = false;
        hitTarget.Clear();
        hitAble = false;
        actionName = "";
    }
    void OnTriggerEnter(Collider other)
    {
        tryHit(other.gameObject);
    }
    void OnTriggerStay(Collider other)
    {
        tryHit(other.gameObject);
    }
    void tryHit(GameObject Object)
    {
        if (!hitAble) return;
        IDamageable target = Object.GetComponent<IDamageable>();
        if (target == null) return;
        if (hitTarget.Contains(target)) return;
        

        hitTarget.Add(target);
        hitLogic(target);
        
    }
    void hitLogic(IDamageable target)
    {
        float attack = PlayerRuntime.Instance.getAttack();
        float mutiply=PlayerRuntime.Instance.baseWeapon.attackDamageMultiplier(actionName);
        //角色总攻击力 * 角色配备武器的so里的招式伤害倍率
        float penetration = PlayerRuntime.Instance.getPenetration();
        target.TakeDamage(new DamageContext(attack, penetration, gameObject));
        Debug.Log("角色"+gameObject.name+"使用"+PlayerRuntime.Instance.baseWeapon.weaponName+"造成了"+attack+"* "+mutiply+"点伤害，穿透率"+penetration);
        //TODO攻击特效，音效
    }
}
