using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class change_scene : MonoBehaviour
{
    public Slider sl;
    public CanvasGroup CG;
    void Start()
    {
        sl.value = 0f;
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(String scenename)
    {
        StartCoroutine(ILS(scenename));
    }
    IEnumerator ILS(String scenename)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenename);
        operation.allowSceneActivation = false;
        CG.alpha = 1;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (sl != null)
            {
                sl.value = progress;
            }
            if (progress >= 1)//只有在滑条加载完全了再切换
            {
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LoadScene("Scene2");
        }
    }
}
