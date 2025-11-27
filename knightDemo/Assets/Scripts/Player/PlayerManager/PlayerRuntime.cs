using System;
using UnityEngine;

public class PlayerRuntime : MonoBehaviour,HealthProvider//这个脚本使用角色和武器的so，并保存角色当前生命值，耐力值等可变属性
{
    public static PlayerRuntime Instance { get; private set; }
    [Header("引用配置")]
    public BasePlayerConfig basePlayer;
    public BaseWeaponConfig baseWeapon;
    public HealthManager health;//通用生命脚本
    public StateManager stateManager;

    [Header("运行时变化属性")]

    public float currentStamina;
    public bool blocking;
    void Awake()//单例初始化配置
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);//带有该脚本的物体实例只能存在一个
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!health) health = GetComponent<HealthManager>();
        if (!stateManager) stateManager = GetComponent<StateManager>();

        currentStamina = basePlayer.Stamina;
    }
        void OnEnable()
    {
        health.GoDied += DiedMethod;
        health.OnInjured += InjuredMethod;
    }
    void OnDisable()
    {
        health.GoDied -= DiedMethod;
        health.OnInjured -= InjuredMethod;
    }
    public float getAttack()
    {
        return basePlayer.Attack + baseWeapon.Attack;//后期各种buff计算也在这里完成
    }
    public float getPenetration()
    {
        return baseWeapon.Penetration;
    }

    public float getHealth()
    {
        return basePlayer.Health;
    }

    public float getDefense()
    {
        return basePlayer.Defense;//TODO以后有防具的代码还可以再写
    }
    private void DiedMethod()//死亡方法，由Health脚本的委托调用
    {

    }
    private void InjuredMethod(DamageContext damageContext)//受伤方法，由Health脚本的委托调用
    {
        if (stateManager == null || damageContext.damageSource == null) return;
        stateManager.change_state(new AttackedState(stateManager, damageContext.damageSource.transform.position));
    }
}
