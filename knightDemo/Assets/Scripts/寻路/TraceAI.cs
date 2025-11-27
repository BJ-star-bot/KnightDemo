using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class TraceAI : MonoBehaviour
{
    public Detect detect;                   // 视野组件
    public Transform target;                // 跟踪目标
    public float refreshInterval = 0.2f;    // 重新寻路的刷新频率
    public float forgetTime = 2f;           // 目标离开视野后保持追踪的时间

    private UnitMover mover;
    private float chaseTimer;
    private float refreshTimer;

    void Awake()
    {
        mover = GetComponent<UnitMover>();
        if (detect == null)
            detect = GetComponentInChildren<Detect>();
    }

    void Update()
    {
        if (target == null || detect == null || mover == null)
            return;

        bool canSeeTarget = detect.CheckVisible(target);
        

        if (canSeeTarget)
        {
            chaseTimer = forgetTime;
        }
        else
        {
            chaseTimer -= Time.deltaTime;
        }

        if (chaseTimer > 0f)
        {
            refreshTimer -= Time.deltaTime;
            if (refreshTimer <= 0f)
            {
                mover.StartTracing(target.position);
                refreshTimer = refreshInterval;
            }
        }
        else
        {
            mover.StopTracing();
        }
    }
}
