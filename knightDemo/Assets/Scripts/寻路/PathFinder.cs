using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PathFinder : MonoBehaviour
{
    private GridManager grid;
    private List<Node> lastPath;

    void Awake()
    {
        grid = GetComponent<GridManager>();
    }
    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        RefreshNode(startNode);
        RefreshNode(targetNode);//刷新起终点，防止起终点被移动障碍盖住导致寻路崩溃

        if (startNode == null || targetNode == null||!startNode.walkable||!targetNode.walkable)
        {
            lastPath = null;
            return null;
        }

        List<Node> openSet = new List<Node>(); //待检查节点
        HashSet<Node> closedSet = new HashSet<Node>(); //已处理节点

        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            RefreshNode(currentNode);//刷新节点及邻居是否可走，应对移动物体，性能消耗低

            if (currentNode == targetNode)
            {
                lastPath = RetracePath(startNode, targetNode);
                return lastPath;
            }

            foreach (Node neighour in grid.GetWalkableNeighbours(currentNode))
            {
                if ( closedSet.Contains(neighour))
                    continue;

                int newCost = currentNode.gCost + GetDistance(currentNode, neighour);
                if (newCost < neighour.gCost || !openSet.Contains(neighour))
                {
                    neighour.gCost = newCost;
                    neighour.hCost = GetDistance(neighour, targetNode);
                    neighour.parent = currentNode;

                    if (!openSet.Contains(neighour))
                        openSet.Add(neighour);
                }
            }
        }

        lastPath = null;
        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;
        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Add(startNode);
        path.Reverse();
        return path;
    }

    private int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        if (dx > dy)
            return 14 * dy + 10 * (dx - dy); //这里的单位随便
        else
            return 14 * dx + 10 * (dy - dx);
    }
    public void RefreshNode(Node node)
    {
        grid.RefreshNode(node);
    }

    void OnDrawGizmos()
    {
        if (lastPath == null || lastPath.Count < 2)
            return;

        Gizmos.color = Color.green;
        Vector3 offset = Vector3.up * 1f;
        float sphereRadius = 0.4f;
        for (int i = 0; i < lastPath.Count - 1; i++)
        {
            Gizmos.DrawLine(lastPath[i].worldPos + offset, lastPath[i + 1].worldPos + offset);
            Gizmos.DrawSphere(lastPath[i].worldPos + offset, sphereRadius);
        }
        Gizmos.DrawSphere(lastPath[lastPath.Count - 1].worldPos + offset, sphereRadius);

#if UNITY_EDITOR
        Vector3[] linePoints = new Vector3[lastPath.Count];
        for (int i = 0; i < lastPath.Count; i++)
            linePoints[i] = lastPath[i].worldPos + offset;

        Color prevColor = Handles.color;
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(8f, linePoints);
        Handles.color = prevColor;
#endif
    }
}
