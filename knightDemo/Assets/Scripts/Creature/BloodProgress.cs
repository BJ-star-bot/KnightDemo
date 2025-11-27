
using UnityEngine;
using UnityEngine.UI;
public class BloodProgress : MonoBehaviour//管理creature的生命条显示
{
    public HealthManager health;

    public UnityEngine.UI.Image RealProgress;
    public UnityEngine.UI.Image LateProgress;
    public float LateSpeed = 0.005f;

    void LateUpdate()
    {

        float per = health.currentHealth / health.Hp.getHealth();
        RealProgress.fillAmount = per;
        LateProgress.fillAmount = Mathf.Lerp(LateProgress.fillAmount, per, LateSpeed);

        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
