using UnityEngine;

public interface HealthProvider //提供生命值和防御力的接口，链接不同生物和Health脚本的桥梁
// 在生物主脚本中实现这个接口并提供生命值和防御力
{
    public float getHealth();
    public float getDefense();
}
