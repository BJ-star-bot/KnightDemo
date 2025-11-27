using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public float maxSpeed = 10f;
    
    public float turnSpeed =5f;
    public float stopDis =0.05f;
    
    public PathFinder pathFinder;
    private Vector3 target;
    public LayerMask groundLayer;
    private Coroutine activeFindPath;
    private bool reFindPath=false;
    public bool mouseDebug=false;
    public bool IsTracing => activeFindPath != null;
    public Vector3 CurrentDestination => target;

    void Update()
    {
        if(pathFinder==null)return;
        if (Input.GetMouseButtonDown(0)&&mouseDebug)
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
            {
                StartTracing(hitInfo.point);
            }
        }
        if (reFindPath)
        {
            List<Node> path=pathFinder.FindPath(transform.position,target);
            if (path != null )
            {
                if(activeFindPath!=null)StopCoroutine(activeFindPath);
                activeFindPath=StartCoroutine(FollowPath(path,target));
            }
            reFindPath=false;
        }
    }
    IEnumerator FollowPath(List<Node> path,Vector3 accurateTarget)
    {
        float currentSpeed=maxSpeed;
        
        for(int i=1;i<path.Count;i++)
        {
            pathFinder.RefreshNode(path[i]);
            if(!path[i].walkable){
                reFindPath=true; //在遇到路径被阻挡后就重新寻路
                activeFindPath=null;
                yield break;
                }

            Vector3 targetPos=path[i].worldPos;
            if(i==path.Count-1)targetPos=accurateTarget;//在最后一格走到鼠标点击精确位置
            

            targetPos.y=transform.position.y;//忽略y值

            Quaternion lookAt=Quaternion.LookRotation(targetPos-transform.position);
            while (Vector3.Distance(transform.position, targetPos) > stopDis)
            {
                transform.rotation=Quaternion.Slerp(transform.rotation,lookAt,turnSpeed*Time.deltaTime);

                transform.position=Vector3.MoveTowards(
                    transform.position,
                    targetPos,
                    currentSpeed*Time.deltaTime
                );
                
                yield return null;
                
            }
        }
        activeFindPath=null;
        reFindPath=false;
    }
    public void StopTracing()
    {
        if(activeFindPath!=null)StopCoroutine(activeFindPath);
        reFindPath=false;
        activeFindPath=null;

    }
    public void StartTracing(Vector3 targetPosition)
    {
        target = targetPosition;
        List<Node> path=pathFinder.FindPath(transform.position,target);
        if (path != null )
            {
            if(activeFindPath!=null)StopCoroutine(activeFindPath);
            activeFindPath=StartCoroutine(FollowPath(path,target));
            }        
    }
}
