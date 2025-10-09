using System;
using UnityEngine;

public class PlayerRuntime : MonoBehaviour
{
    public static PlayerRuntime Instance { get; private set; }
    [Header("引用配置")]
    public BasePlayerConfig basePlayer;
    public BaseWeaponConfig baseWeapon;
    [Header("运行时变化属性")]
    public float currentHealth;
    public float currentStamina;

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
        currentHealth = basePlayer.Health;
        currentStamina = basePlayer.Stamina;

    }
    public float getAttack()
    {
        return basePlayer.Attack + baseWeapon.Attack;//后期各种buff计算也在这里完成
    }
    public float getPenetration()
    {
        return baseWeapon.Penetration;
    }
}
