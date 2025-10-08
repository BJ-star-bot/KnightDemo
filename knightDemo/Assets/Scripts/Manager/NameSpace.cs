
using UnityEngine;

namespace TestGame.Dialogue
{
    [System.Serializable]
    public struct Sentence
    {
        public bool IsLeft;
        public string Name;
        public string Line;
    }

}
namespace TestGame.Movement
{
    [System.Serializable]
    public struct AniInformation
    {
        public string AniName;
        public string AttackLayer;//比default更上层的layer，控制角色攻击时上半身动作
        public float MoveX;
        public float MoveY;
        public AniInformation(string a, string b="",float c = 0, float d = 0)
        {
            AniName = a;
            AttackLayer = b;
            MoveX = c;
            MoveY = d;
        }
    }
}
namespace TestGame.Combat
{
    [System.Serializable]
    public struct DamageSource
    {
        public string Name;
        public Vector3 Position;
        public float DamageAmount;
        public DamageSource(string a, Vector3 b, float c)
        {
            Name = a;
            Position = b;
            DamageAmount = c;
        }
    }
}