using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个频道的消息显示，负责维护内容高度、滚动等。
/// </summary>
public class InfoConsoleChannel : MonoBehaviour
{
    [SerializeField, Tooltip("频道名称，供 UI 显示。")]
    private string channelName = "Channel";
    [SerializeField, Tooltip("滚动控件，用于维持滚动位置。")]
    private ScrollRect scrollRect;
    [SerializeField, Tooltip("用于放置日志行的 Content。")]
    private RectTransform contentRoot;
    [SerializeField, Tooltip("每条日志的 TextMeshPro 预制体，需保持隐藏状态。")]
    private TMP_Text linePrefab;
    [SerializeField, Min(1), Tooltip("最多保留的消息数量。")]
    private int maxLines = 80;
    [SerializeField, Tooltip("内容上下内边距（像素）。")]
    private float verticalPadding = 12f;
    [SerializeField, Tooltip("行与行之间的额外距离（像素）。")]
    private float lineSpacing = 4f;
    [SerializeField, Range(0f, 1f), Tooltip("只有当滚动条低于该阈值时才自动跟随到底部。")]
    private float autoScrollThreshold = 0.05f;

    private readonly Queue<TMP_Text> pooledLines = new Queue<TMP_Text>();
    private readonly List<TMP_Text> activeLines = new List<TMP_Text>();

    public string ChannelName => channelName;

    private void Awake()
    {
        if (scrollRect == null)
        {
            scrollRect = GetComponentInChildren<ScrollRect>(true);
        }
        if (contentRoot == null && scrollRect != null)
        {
            contentRoot = scrollRect.content;
        }
    }

    public void Clear()
    {
        foreach (var line in activeLines)
        {
            if (line != null)
            {
                line.gameObject.SetActive(false);
                pooledLines.Enqueue(line);
            }
        }
        activeLines.Clear();
        UpdateLayout();
    }

    public void AddMessage(string rawText, LogType type = LogType.Log, string timestampFormat = "HH:mm:ss", bool includeTimestamp = true)
    {
        var text = includeTimestamp ? FormatMessage(rawText, type, timestampFormat) : rawText;
        AddFormattedMessage(text);
    }

    public void AddFormattedMessage(string formattedText)
    {
        if (linePrefab == null || contentRoot == null)
        {
            return;
        }

        var entry = GetLineInstance();
        entry.text = formattedText;
        entry.gameObject.SetActive(true);
        activeLines.Add(entry);

        if (activeLines.Count > maxLines)
        {
            var old = activeLines[0];
            activeLines.RemoveAt(0);
            old.gameObject.SetActive(false);
            pooledLines.Enqueue(old);
        }

        var autoFollow = ShouldAutoScroll();
        UpdateLayout();
        if (autoFollow)
        {
            ScrollToBottom();
        }
    }

    public void SetActiveView(bool visible)
    {
        gameObject.SetActive(visible);
        if (visible)
        {
            UpdateLayout();
            ScrollToBottom();
        }
    }

    private TMP_Text GetLineInstance()
    {
        TMP_Text line = null;
        while (pooledLines.Count > 0 && line == null)
        {
            line = pooledLines.Dequeue();
        }

        if (line == null)
        {
            line = Instantiate(linePrefab, contentRoot);
            line.gameObject.SetActive(false);
            PrepareRect(line.rectTransform);
            line.textWrappingMode = TextWrappingModes.Normal;
            line.overflowMode = TextOverflowModes.Overflow;
        }

        return line;
    }

    private void UpdateLayout()
    {
        if (contentRoot == null)
        {
            return;
        }

        float height = verticalPadding;
        float currentY = -verticalPadding;
        var count = 0;
        foreach (var line in activeLines)
        {
            if (line == null || !line.gameObject.activeSelf)
                continue;

            count++;

            var rect = line.rectTransform;
            PrepareRect(rect);

            // 关键：先刷新 TMP 的 mesh 再拿正确高度
            line.ForceMeshUpdate();    
            float rectHeight = line.preferredHeight;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectHeight);

            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x, 
                currentY
            );

            currentY -= rectHeight + lineSpacing;
            height += rectHeight + lineSpacing;
        }


        if (count > 0)
        {
            height -= lineSpacing;
        }

        height += verticalPadding;
        if (scrollRect != null && scrollRect.viewport != null)
        {
            height = Mathf.Max(height, scrollRect.viewport.rect.height);
        }

        contentRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    private void ScrollToBottom()
    {
        if (scrollRect == null)
        {
            return;
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    private bool ShouldAutoScroll()
    {
        if (scrollRect == null)
        {
            return true;
        }

        // verticalNormalizedPosition 越接近 0 越靠近底部
        return scrollRect.verticalNormalizedPosition <= autoScrollThreshold;
    }

    private static void PrepareRect(RectTransform rect)
    {
        if (rect == null)
        {
            return;
        }

        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.offsetMin = new Vector2(0f, rect.offsetMin.y);
        rect.offsetMax = new Vector2(0f, rect.offsetMax.y);
    }

    private static string FormatMessage(string message, LogType type, string timestampFormat)
    {
        var color = type switch
        {
            LogType.Warning => "#ffcc00",
            LogType.Error => "#ff5555",
            LogType.Exception => "#ff5555",
            _ => "#ffffff"
        };

        var timestamp = System.DateTime.Now.ToString(timestampFormat);
        return $"<color=#888888>[{timestamp}]</color> <color={color}>{message}</color>";
    }
}
