using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public Transform swordTip;
    public Transform swordBase;
    private Vector3 lastTipPos;
    private Vector3 lastBasePos;
    private float currentSpeed;
    public float minSpeed = 0f;
    void Update()
    {
        Vector3 tipVel = (swordTip.position - lastTipPos) / Time.deltaTime;
        Vector3 baseVel = (swordBase.position - lastBasePos) / Time.deltaTime;
        currentSpeed = (tipVel.magnitude + baseVel.magnitude) * 0.5f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")&&currentSpeed>minSpeed)
        {
            other.GetComponent<BaseEnemy>()?.TakeDamage(new DamageContext(50, gameObject));
      }
    }

}
