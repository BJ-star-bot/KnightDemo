using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveRecord
{
    public string id;
    public string type;
    public string json;
}
[Serializable]
public class SaveFile//这个类是最终会被转为json储存的类，包含了当前场景名，和储存列表
{
    public string sceneName;
    public List<SaveRecord> records = new List<SaveRecord>();//List是动态数组，这里保存了各个需保存内容的类
}
public class SaveManager : MonoBehaviour
{
    string path => Path.Combine(Application.persistentDataPath, "save.json");//表达式主体成员，每次调用path都会重新返回右边表达式的值。path本身不储存值，是只读属性
    public void SaveGame()
    {
        var sf = new SaveFile { sceneName = SceneManager.GetActiveScene().name };
        foreach (var saveable in FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>())//找到所有脚本组件，包括未激活的。在这些脚本组件中找到实现了ISaveable接口的
        {
            var stateObj = saveable.CaptureState();
            if (stateObj == null) continue;
            sf.records.Add(new SaveRecord
            {
                id = saveable.GetUniqueId(),
                type = stateObj.GetType().AssemblyQualifiedName,//这里是获取需要储存的类的标识名，用于之后load时恢复类，如果不储存这个信息，在load时就不知道把json恢复成什么东西
                json = JsonUtility.ToJson(stateObj)
            });
        }
        string json = JsonUtility.ToJson(sf, true);
        File.WriteAllText(path, json);//把sf的json格式写入存档路径
#if UNITY_EDITOR//条件编译，这里的代码只有在unity编辑器里才会执行，打包后不执行
        Debug.Log($"[save]{path}\n{json}");
#endif
    }
    public void LoadGame() {
        if (!File.Exists(path)) { Debug.LogWarning("存档不存在"); return; }
        string json = File.ReadAllText(path);
        var sf = JsonUtility.FromJson<SaveFile>(json);//把json按savefile类格式解析
        if (sf.sceneName != SceneManager.GetActiveScene().name)
        {
            Debug.LogWarning($"当前场景（{SceneManager.GetActiveScene().name}）与存档场景（{sf.sceneName}）不一致");
            return;
        }
        int applied = 0;
        foreach (var rec in sf.records)
        {
            var target = FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include, FindObjectsSortMode.None)
                .OfType<ISaveable>()
                .FirstOrDefault(s => s.GetUniqueId() == rec.id);
            if (target == null) continue;
            var t = Type.GetType(rec.type);//从list中获取当前的类型
            if (t == null)
            {
                Debug.LogWarning($"类型解析失败"); continue;
            }
            var stateObj = JsonUtility.FromJson(rec.json, t);//按类型解析list中的json，返回t类型的类
            target.RestoreState(stateObj);//接口的load方法实现，看具体实现方法
            applied++;

        }
#if UNITY_EDITOR
        Debug.Log($"[Load]应用{applied}/{sf.records.Count}条记录");
#endif
        
   }
}

