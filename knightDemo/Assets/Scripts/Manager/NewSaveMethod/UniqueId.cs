using UnityEngine;
using System;

[DisallowMultipleComponent]
public class UniqueId : MonoBehaviour
{
    [SerializeField] string uniqueId = Guid.NewGuid().ToString(); // 初次生成

    public string Id => uniqueId;

    // 确保编辑器里不会每次都改 ID
    void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueId))
        {
            uniqueId = Guid.NewGuid().ToString();
        }
    }

}
