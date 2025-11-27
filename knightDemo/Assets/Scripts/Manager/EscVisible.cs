using UnityEngine;
using UnityEngine.UI;

public class EscVisible : MonoBehaviour
{
    [SerializeField]
    public CanvasGroup buttons;//ui辅助组件，控制ui可见性和可交互性
    public Button saveButton;
    public Button loadButton;
    void showPanel()
    {
        buttons.alpha = 1;
        buttons.interactable = true;//ui看得见但点不动，会阻挡下层ui
        buttons.blocksRaycasts = true;//ui看得见，但点击会点到下层ui
    }
    void hidePanel()
    {
        buttons.alpha = 0;
        buttons.interactable = false;
        buttons.blocksRaycasts = false;
    }
    void Start()
    {
        hidePanel();
        saveButton.onClick.AddListener(hidePanel);//事件是对外暴露‘订阅/退订’的委托
        loadButton.onClick.AddListener(hidePanel);//事件能带一条‘委托链’，调用时依次执行。这里onclick就是unity内置的事件，hidepanel就是手动添加的委托链之一
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buttons.alpha == 1) hidePanel();
            else showPanel();
        }
        
    }
}
