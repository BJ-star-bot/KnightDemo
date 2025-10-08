using Unity.VisualScripting;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public SaveManager saveManager;
    public void click_save()
    {
        saveManager.SaveGame();
    }
}
