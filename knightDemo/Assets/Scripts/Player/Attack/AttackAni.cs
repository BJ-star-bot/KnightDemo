using UnityEngine;

public class AttackAni : MonoBehaviour//这个脚本挂在有anicontroller的物体下，用来接受ani event
{
    public AttackDetect Ad;
    public void hitEnable(string id)
    {
        print("hit");
        Ad.enableHitBox(id);
    }
    public void hitDisable()
    {
        Ad.disableHitBox();
    }
}
