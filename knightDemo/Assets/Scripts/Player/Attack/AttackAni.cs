using UnityEngine;

public class AttackAni : MonoBehaviour//这个脚本挂在有anicontroller的物体下，用来接受ani event
{
    public AttackDetect Ad;
    public void HitEnable(string id)//一段攻击开始时触发
    {
        print("hit");
        Ad.enableHitBox(id);
    }
    public void HitDisable()//一段攻击结束时触发
    {
        Ad.disableHitBox();
    }
    public void Finish()//全部攻击动画结束触发
    {
        
    }
}
