using UnityEngine;
[CreateAssetMenu(menuName = "Config/BaseWeaponConfig")]
public class BaseWeaponConfig : ScriptableObject
{
    [Header("基础属性")]
    public string weaponName = "基础长剑";
    public float Attack = 10;
    public float Durability = 1; //耐久度
    public float Penetration = 10; //穿透率
                                   //实际穿透= 穿透率 * （1-耐久度/2）
    public float minSpeed;
    public float attackDamageMultiplier(string id)//招式倍率
    {
        switch (id)
        {
            case "light"://轻击
                {
                    return 1f;
                }
            case "heavy":
                {
                    return 1.5f;
                }
            case "downward"://跳斩/下击
                {
                    return 1.8f;
            }
        }
        return 1;
    }
}
