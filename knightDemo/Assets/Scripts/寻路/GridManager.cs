using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector2 gridWorldSize=new Vector2(20,20);//整体网格在世界中大小
    public float nodeRadius=0.5f;//每个小格的半径
    public LayerMask unWalkableMask;
    Node[,] grid;
    float nodeDiameter;
    int gridSizeX,gridSizeY;
    Vector3 startPos;

    void Awake()
    {
        nodeDiameter=nodeRadius*2;
        gridSizeX=Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY=Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        startPos=transform.position
        -Vector3.right*gridWorldSize.x/2
        -Vector3.forward*gridWorldSize.y/2;

        CreateGrid();

    }
    void CreateGrid()
    {
        grid=new Node[gridSizeX,gridSizeY];

        for(int i = 0; i < gridSizeX; i++)
        {
            for(int j = 0; j < gridSizeY; j++)
            {
                Vector3 currentPos=startPos
                +Vector3.right*(i*nodeDiameter+nodeRadius)
                +Vector3.forward*(j*nodeDiameter+nodeRadius);
                bool walkable=!Physics.CheckSphere(currentPos,nodeRadius,unWalkableMask);
                grid[i,j]=new Node(walkable,currentPos,i,j);
            }
        }
    }
    public void RefreshNode(Node node)
    {
        foreach (Node a in GetAllNeighbours(node,true))
        {
            if(!isInside(a.gridX,a.gridY))continue;
            grid[a.gridX,a.gridY].walkable=!Physics.CheckSphere(a.worldPos,nodeRadius,unWalkableMask);
        }
    }
    public Node NodeFromWorldPoint(Vector3 worldPos)//返回世界坐标对应的点
    {
        worldPos-=startPos;
        if(worldPos.x<0||worldPos.x>gridWorldSize.x)return null;
        if(worldPos.z<0||worldPos.z>gridWorldSize.y)return null;
        
        int x=Mathf.Clamp(Mathf.RoundToInt(worldPos.x/nodeDiameter),0,gridSizeX-1);
        int y=Mathf.Clamp(Mathf.RoundToInt(worldPos.z/nodeDiameter),0,gridSizeY-1);
        
        return grid[x,y];
    }
    public List<Node> GetWalkableNeighbours(Node node)//返回可走邻居点
    {
        List<Node> result=new List<Node>();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if(i==0&&j==0)continue;
                int ni=node.gridX+i;
                int nj=node.gridY+j;
                if(!isInside(ni,nj))continue;

                if (i != 0 && j != 0)//对于斜角邻居，只要有一侧不可走就认为不可走
                {//在过墙∠时表现也更自然
                    if(!isInside(ni,node.gridY)||!grid[ni,node.gridY].walkable)continue;
                    if(!isInside(node.gridX,nj)||!grid[node.gridX,nj].walkable)continue;
                }
                if(!grid[ni,nj].walkable)continue;
                result.Add(grid[ni,nj]);
            }
        }
        return result;
    }
    
    private bool isInside(int x,int y)
    {
        return x>=0&&x<gridSizeX&&y>=0&&y<gridSizeY;
    }
    
    public List<Node> GetAllNeighbours(Node node,bool includeSelf=false)//获取所有邻居，不论可否走
    {
        List<Node> result=new List<Node>();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if(i==0&&j==0&&!includeSelf)continue;
                int ni=node.gridX+i;
                int nj=node.gridY+j;
                if(!isInside(ni,nj))continue;
                result.Add(grid[ni,nj]);
            }
        }
        return result;
    }
   
   void OnDrawGizmos()
    {
        Gizmos.color=Color.grey;
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(gridWorldSize.x,1f,gridWorldSize.y)
        );
                if (grid == null) return;

        float nodeDiameterLocal = nodeRadius * 2f;
        Vector3 cubeSize = Vector3.one * (nodeDiameterLocal - 0.05f);

        foreach (Node n in grid)
        {
            Gizmos.color = n.walkable ? Color.white : Color.red;
            Gizmos.DrawCube(n.worldPos, cubeSize);
        }
    }
    

}
