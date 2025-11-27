using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    private Vector3 start;
    public Transform target;
    private Vector3 targetPos;
    public float speed=5f;
    public float sleepTime=5f;
    private Coroutine sleepCo;

    void Awake()
    {
        start=transform.position;
        if(target==null)return;
        targetPos=target.position;
        targetPos.y=transform.position.y;
    }
    void Update()
    {
        if(target==null)return;
        if (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position=Vector3.MoveTowards(transform.position,targetPos,speed*Time.deltaTime);
        }
        else
        {
            if (sleepCo == null)sleepCo=StartCoroutine(Sleep(sleepTime));
        }
    }
    IEnumerator Sleep(float sleepTime)
    {
        yield return new WaitForSeconds(sleepTime);
        targetPos=start;
        start=transform.position;
        sleepCo=null;
    }
}
