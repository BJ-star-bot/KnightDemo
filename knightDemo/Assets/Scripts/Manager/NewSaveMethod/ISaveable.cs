using UnityEngine;

public interface ISaveable
{
    object CaptureState();//导出自己的状态
    void RestoreState(object state);//读档时存入自己的状态.定义接口的作用就是让物体自己描述该如何save和load，而不用系统去关心这个事
    string GetUniqueId();//定义唯一ID
}