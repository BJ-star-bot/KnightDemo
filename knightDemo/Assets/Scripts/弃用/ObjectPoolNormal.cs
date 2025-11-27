using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolNormal<T> where T:new()//这个对象池是c#通用对象池,但不完全适用于unity
//这里where要求类型T必须有无参数的构造函数，以便对象池初始化
{
    private Queue<T> pool;
    private int maxSize;
    public ObjectPoolNormal(int size)
    {
        pool = new Queue<T>();
        maxSize = size;
        for (int i = 0; i < size; i++)
        {
            pool.Enqueue(new T());
        }
    }
    public T GetObject()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            ExpandPool(1);
            return pool.Dequeue();
        }
    }
    public void ReturnObject(T Dobject)
    {
        if (pool.Count < maxSize)
        {
            pool.Enqueue(Dobject);
        }
        else
        {
            //池满时策略

        }
    }
    public void ExpandPool(int newObjectCount)
    {
        for (int i = 0; i < newObjectCount; i++)
        {
            pool.Enqueue(new T());
        }
        maxSize += newObjectCount;
    }
}
