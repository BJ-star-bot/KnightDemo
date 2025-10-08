using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using TestGame.Dialogue;
using System.Collections.Generic;
public class DIalogueManager : MonoBehaviour
{
    void Start()
    {
        Hide();
    }
    public CanvasGroup canvas_group;
    public TMP_Text Sentence;
    public TMP_Text SpeakerName;
    public float LeftPosition = -500f;
    public float RightPosition = 1300f;
    int index = 0;
    public float TapSpeed = 10f;
    private bool IsDialogPlaying;
    public List<Sentence> CurrentDialogue;
    public void MakeNewDialogue(List<Sentence> Dialogue)
    {
        CurrentDialogue = Dialogue;
        index = 0;
        Show();
        IsDialogPlaying = true;
        PlayNextSentence();
    }
    public void PlayNextSentence()
    {
        if (CurrentDialogue != null)
        {
            index += 1;
            if (index >= CurrentDialogue.Count) { EndDialogue(); return; }
            StartCoroutine(TapWord(Sentence, CurrentDialogue[index].Line, TapSpeed));
            SpeakerName.text = CurrentDialogue[index].Name;

            Vector3 p = SpeakerName.transform.position;
            if (CurrentDialogue[index].IsLeft)
                p.x = LeftPosition;
            else p.x = RightPosition;
            SpeakerName.transform.position = p;

        }
    }
    public void EndDialogue()
    {
        CurrentDialogue = null;
        IsDialogPlaying = false;
        Hide();
    }
    void Update()
    {
        if (IsDialogPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayNextSentence();
            }
        }
    }
    private void Show()
    {
        canvas_group.alpha = 1f;
        canvas_group.interactable = true;
        canvas_group.blocksRaycasts = true;
    }
    private void Hide()
    {
        canvas_group.alpha = 0f;
        canvas_group.interactable = false;
        canvas_group.blocksRaycasts = false;
    }
    IEnumerator TapWord(TMP_Text tmp, string line,float speed)
    {
        tmp.text = "";
        int len = 0;
        float T = 0f;
        while (len < line.Length)
        {
            T += Time.unscaledDeltaTime * speed;
            int n = Mathf.FloorToInt(T);
            if (n> len)
            {
                len = n;
                tmp.text = line.Substring(0, len);
            }
            yield return null;
        }

        
    }
}
