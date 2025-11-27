using TMPro;
using UnityEngine;

public class text_score : MonoBehaviour
{
    public TMP_Text score_text;
    public static text_score Instance;//创建一个静态的实例，指向本类，可以让本类被全局调用

    int score = 0;
   
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        update_text();

    }



    public void add_score(int s=1)
    {
        score += s;
        update_text();
    }
    void update_text()
    {
        score_text.text = "score : " + score;
    }
}
