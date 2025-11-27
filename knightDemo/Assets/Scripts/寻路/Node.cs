using UnityEngine;
public class Node 
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;
    //
    public int gCost; //起点到当前点的代价
    public int hCost;//当前点到终点的代价
    public Node parent;
    public int fCost=>gCost+hCost;//通过当前点链接起点和终点的长度
    public Node(bool walkable,Vector3 worldPos,int gridX,int gridY)
    {
        this.walkable=walkable;
        this.worldPos=worldPos;
        this.gridX=gridX;
        this.gridY=gridY;

    }
}
