using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour, ISaveable
{
    [SerializeField] private UniqueId uniqueId;
    public TMP_Text tmp;
    public string text;
    void Start()
    {
        text = "Closed";
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text = "Opened";

        }
    }
    void Update()
    {
        tmp.text = text;
    }

    public object CaptureState()
    {
        return new ChestData{opened=text};
    }
    public void RestoreState(object state)
    {
        var data = (ChestData)state;
        text = data.opened;
    }
    public string GetUniqueId() => uniqueId.Id;
    [System.Serializable]
    public struct ChestData
    {
        public string opened;
    }
}
