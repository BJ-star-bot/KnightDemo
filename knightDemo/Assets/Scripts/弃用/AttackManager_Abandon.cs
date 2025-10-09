using UnityEngine;
namespace Abandon
{
    public class AttackManager_UseSpeed : MonoBehaviour//这版是用speed计算攻击的，但不利于之后模块化的开发，先弃用
    {
        public Transform swordTip;
        public Transform swordBase;
        private Vector3 lastTipPos;
        private Vector3 lastBasePos;
        private float currentSpeed = 0f;
        public float minSpeed = 0f;
        void Update()
        {
            if (!swordTip || !swordBase) return;
            Vector3 tipVel = (swordTip.position - lastTipPos) / Time.deltaTime;
            Vector3 baseVel = (swordBase.position - lastBasePos) / Time.deltaTime;
            currentSpeed = (tipVel.magnitude + baseVel.magnitude) * 0.5f;//根据武器平均速度结算伤害
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && currentSpeed > minSpeed)
            {
                other.GetComponent<BaseEnemy>()?.TakeDamage(new DamageContext(50, 0,gameObject));

            }
        }
        public void TestAni()
        {
            print("ani");
        }
    }
}