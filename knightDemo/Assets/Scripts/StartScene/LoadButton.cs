using Unity.VisualScripting;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public SaveManager saveManager;
    public void click_load()
    {
        saveManager.LoadGame();
    }
}