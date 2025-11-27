using UnityEngine;

[CreateAssetMenu(menuName = "Config/BaseCreature")]
public class BaseCreatureConfig : ScriptableObject
{
    [Header("基础生物")]
    public  float health=100f;
    public float defense=10f;
    public float speed=5f;
    public float attack = 20f;
    public float penetration = 0f;
}
