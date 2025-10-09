using UnityEngine;
[CreateAssetMenu(menuName = "Config/BasePlayer")]
public class BasePlayerConfig : ScriptableObject
{
    [Header("移动属性")]
    public float Health=100f;
    public float WalkSpeed=2f;
    public float RunSpeed=4f;
    public float TurnAroundspeed = 7f;
    public float JumpSpeed = 4f;

    [Header("战斗属性")]
    public float Attack = 10f;
    public float Defense = 10f;
    //有效防御 = 防御力 × (1 - 穿透率)
    //实际伤害 = 攻击力 * 招式倍率 - 有效防御 + 保底伤害。

    
    [Header("耐力属性")]
    public float Stamina=100f;//耐力,只有在战斗状态下才会消耗耐力，常规耐力无限
    public float RunCostPerSec=15f;//跑步每秒消耗耐力值，和重量挂钩
    public float JumpCost=20f;//跳跃消耗耐力
    public float StaminaRecover = 30f;//在idle，walk状态下恢复耐力每秒
    public float Weight = 1;//重量，百分比，耐力消耗=基础消耗 * 重量

}
