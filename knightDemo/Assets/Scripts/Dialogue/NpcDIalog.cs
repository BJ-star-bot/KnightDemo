using System.Collections.Generic;
using UnityEngine;
using TestGame.Dialogue;

public class NpcDialog : MonoBehaviour
{
    [SerializeField]
    public List<Sentence> Conversation;
    public DIalogueManager DManager;
    public CanvasGroup Notice;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Notice.alpha = 1f;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Notice.alpha = 0f;
            
        }
    }
    void Start()
    {
        Notice.alpha = 0f;
    }
    void Update()
    {
        if (Notice.alpha > 0.9f)
        {
            if (Input.GetKeyDown(KeyCode.C))DManager.MakeNewDialogue(Conversation);
        }
    }
}
