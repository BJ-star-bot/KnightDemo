using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class BloodProgress : MonoBehaviour
{
    public Health hl;

    public UnityEngine.UI.Image blood_progress;
    
    void Start()
    {
        
    }


    void LateUpdate()
    {
        float per = hl.blood / hl.full_blood;
        blood_progress.fillAmount = per;
    }
}
