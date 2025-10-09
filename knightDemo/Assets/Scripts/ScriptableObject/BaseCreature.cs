using UnityEngine;

[CreateAssetMenu(menuName = "BaseCreature")]
public class BaseCreatureConfig : ScriptableObject
{
    [Header("基础生物")]
    public float baseHealth;
    public float baseAttack;
    public float baseDefense;
    public float baseSpeed;
    public float invincibleTime;
}
