
using System.Collections;
using TMPro;
using UnityEngine;

public class BossDefeat : MonoBehaviour
{
    public TMP_Text tmp;
    public float fade_in_time = 1f;
    public float continue_time = 2f;
    public float fade_out_time = 1f;
    private Coroutine playing;
    void OnEnable()
    {
        Boss.Ondied += play;
    }
    void OnDisable()
    {
        Boss.Ondied -= play;
    }
    void Start()
    {
        
        SetAlpha(0);//初始透明
        if (!tmp) tmp = gameObject.GetComponent<TextMeshPro>();
    }
    void play(string word)
    {
        if (playing != null) StopCoroutine(playing);
        playing = StartCoroutine(show_word(word));
    }
    IEnumerator show_word(string word)
    {
        tmp.text = word;
        yield return StartCoroutine(FadeTo(1f, fade_in_time));
        yield return new WaitForSeconds(continue_time);
        yield return StartCoroutine(FadeTo(0f, fade_out_time));
        tmp.text = "";
        playing = null;
    }
    IEnumerator FadeTo(float to, float duration)
    {
        float timer = 0f;
        float from = tmp.color.a;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, timer / duration));//每帧计算一个插值然后挂起，等下一帧继续执行知道到时间
            yield return null;//这样代码不会卡死在这里

        }
        SetAlpha(to);
    }
    void SetAlpha(float a)
    {
        Color c = tmp.color;
        c.a = a;
        tmp.color = c;
    }
}
