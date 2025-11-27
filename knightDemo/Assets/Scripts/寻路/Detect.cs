using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Detect : MonoBehaviour
{
    [Header("视野配置")]
    public float viewRadius = 10f;                 // 视野半径
    [Range(0f, 360f)] public float viewAngle = 90f; // 视野角度
    public LayerMask targetMask;                   // 目标层
    public LayerMask obstacleMask;                 // 障碍层
    [Range(10, 180)] public int rayCount = 60;      // 射线数量（影响扇形精度）
    public float meshYOffset = 0.1f;               // 网格抬升高度，避免和地面重叠

    public bool targetVisible{ get;private set;}                   // 当前目标是否在视野内
    public Transform CurrentTarget => currentTarget;               // 当前锁定的目标
    [ColorUsage(false, true)]
    public Color viewColor = new Color(0f, 1f, 0f, 0.15f);         // 扇形颜色/透明度
    
    private MeshFilter viewMeshFilter;
    private MeshRenderer viewMeshRenderer;
    private Mesh viewMesh;
    private Transform currentTarget;

    void Awake()
    {
        viewMeshFilter = GetComponent<MeshFilter>();
        viewMeshRenderer = GetComponent<MeshRenderer>();
        viewMesh = new Mesh { name = "View Mesh" };
        viewMeshFilter.mesh = viewMesh;
        if (viewMeshRenderer != null)
        {
            Material matInstance = viewMeshRenderer.material;
            if (matInstance != null)
            {
                matInstance.color = viewColor;
            }
        }
    }

    void LateUpdate()
    {
        DrawFieldOfView();                         // 每帧更新视野网格
        UpdateCurrentTarget();
    }

    public bool CheckVisible(Transform target)
    {
        Vector3 offsetPos = transform.position + Vector3.up * meshYOffset;
        Vector3 dirToTarget = (target.position - offsetPos);
        float distance = dirToTarget.magnitude;
        if (distance > viewRadius)
            return false;

        dirToTarget.Normalize();
        float angle = Vector3.Angle(transform.forward, dirToTarget);
        if (angle > viewAngle * 0.5f)
            return false;

        if (Physics.Raycast(offsetPos, dirToTarget, distance, obstacleMask))
            return false;

        return true;
    }

    private void UpdateCurrentTarget()
    {
        currentTarget = null;
        float closestDistance = float.MaxValue;
        Collider[] candidates = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (Collider candidate in candidates)
        {
            Transform target = candidate.transform;
            Vector3 offsetPos = transform.position + Vector3.up * meshYOffset;
            Vector3 dir = target.position - offsetPos;
            float distance = dir.magnitude;
            if (distance > viewRadius) continue;
            dir.Normalize();
            float angle = Vector3.Angle(transform.forward, dir);
            if (angle > viewAngle * 0.5f) continue;
            if (Physics.Raycast(offsetPos, dir, distance, obstacleMask)) continue;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = target;
            }
        }
        targetVisible = currentTarget != null;
    }

    private void DrawFieldOfView()
    {
        float stepAngleSize = viewAngle / (rayCount - 1); // 每条射线的角度间隔
        List<Vector3> viewPoints = new List<Vector3>(rayCount);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle * 0.5f + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.up * meshYOffset;
        for (int i = 0; i < viewPoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < viewPoints.Count - 1)
            {
                int triIndex = i * 3;
                triangles[triIndex] = 0;
                triangles[triIndex + 1] = i + 1;
                triangles[triIndex + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle);
        Vector3 origin = transform.position + Vector3.up * meshYOffset;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance);
        }

        return new ViewCastInfo(false, origin + dir * viewRadius, viewRadius);
    }

    private Vector3 DirFromAngle(float angleInDegrees)
    {
        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
    }

    private struct ViewCastInfo
    {
        public readonly bool hit;
        public readonly Vector3 point;
        public readonly float distance;

        public ViewCastInfo(bool hit, Vector3 point, float distance)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
        }
    }
}
