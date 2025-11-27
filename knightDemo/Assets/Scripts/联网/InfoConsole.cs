using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 信息台总控：负责频道切换、面板显示以及系统/局域网消息入口。
/// </summary>
public class InfoConsole : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField, Tooltip("信息台的 CanvasGroup，用于显示/隐藏。")]
    private CanvasGroup panel;
    [SerializeField, Tooltip("按下该按键时开关信息台。")]
    private KeyCode toggleKey = KeyCode.Tab;

    [Header("Channels")]
    [SerializeField, Tooltip("系统日志频道。")]
    private InfoConsoleChannel systemChannel;
    [SerializeField, Tooltip("局域网聊天频道。")]
    private InfoConsoleChannel lanChannel;
    [SerializeField, Tooltip("当前频道文本显示。")]
    private TMP_Text currentChannelLabel;
    [SerializeField, Tooltip("切换按钮文本（显示下一个频道名称）。")]
    private TMP_Text switchButtonLabel;
    [SerializeField, Tooltip("切换频道的按钮。")]
    private Button switchChannelButton;

    [Header("LAN Input")]
    [SerializeField, Tooltip("局域网频道的输入框。")]
    private TMP_InputField lanInputField;
    [SerializeField, Tooltip("局域网发送按钮。")]
    private Button lanSendButton;
    [SerializeField, Tooltip("发送后前缀，例如 'Me:'。")]
    private string lanSenderPrefix = "我";

    private readonly List<InfoConsoleChannel> channels = new List<InfoConsoleChannel>();
    private InfoConsoleChannel activeChannel;
    private bool panelVisible;

    private void Awake()
    {
        if (panel == null)
        {
            panel = GetComponentInChildren<CanvasGroup>();
        }

        channels.Clear();
        if (systemChannel != null)
        {
            channels.Add(systemChannel);
        }
        if (lanChannel != null)
        {
            channels.Add(lanChannel);
        }

        SetPanelVisible(true);
        SetActiveChannel(systemChannel ?? lanChannel);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleSystemLog;

        if (switchChannelButton != null)
        {
            switchChannelButton.onClick.AddListener(SwitchToNextChannel);
        }
        if (lanSendButton != null)
        {
            lanSendButton.onClick.AddListener(SendLanMessage);
        }
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleSystemLog;

        if (switchChannelButton != null)
        {
            switchChannelButton.onClick.RemoveListener(SwitchToNextChannel);
        }
        if (lanSendButton != null)
        {
            lanSendButton.onClick.RemoveListener(SendLanMessage);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            SetPanelVisible(!panelVisible);
        }
    }

    private void HandleSystemLog(string condition, string stackTrace, LogType type)
    {
        systemChannel?.AddMessage(condition, type);
    }

    private void SwitchToNextChannel()
    {
        if (channels.Count == 0 || activeChannel == null)
        {
            return;
        }

        var index = channels.IndexOf(activeChannel);
        var next = channels[(index + 1) % channels.Count];
        SetActiveChannel(next);
    }

    private void SetActiveChannel(InfoConsoleChannel channel)
    {
        if (channel == null)
        {
            return;
        }

        activeChannel = channel;
        foreach (var entry in channels)
        {
            if (entry == null)
            {
                continue;
            }
            entry.SetActiveView(entry == channel);
        }

        if (currentChannelLabel != null)
        {
            currentChannelLabel.text = channel.ChannelName;
        }
        if (switchButtonLabel != null)
        {
            var next = GetNextChannelName();
            switchButtonLabel.text = string.IsNullOrEmpty(next) ? "切换" : next;
        }
    }

    private string GetNextChannelName()
    {
        if (channels.Count <= 1 || activeChannel == null)
        {
            return string.Empty;
        }

        var index = channels.IndexOf(activeChannel);
        var next = channels[(index + 1) % channels.Count];
        return next != null ? next.ChannelName : string.Empty;
    }

    private void SendLanMessage()
    {
        if (lanChannel == null || lanInputField == null)
        {
            return;
        }

        var text = lanInputField.text.Trim();
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var prefix = string.IsNullOrEmpty(lanSenderPrefix) ? string.Empty : $"{lanSenderPrefix}: ";
        lanChannel.AddMessage(prefix + text, LogType.Log);
        lanInputField.text = string.Empty;
    }

    private void SetPanelVisible(bool visible)
    {
        panelVisible = visible;
        if (panel == null)
        {
            return;
        }

        panel.alpha = visible ? 1f : 0f;
        panel.blocksRaycasts = visible;
        panel.interactable = visible;

        if (visible)
        {
            panel.transform.SetAsLastSibling();
        }
    }
}
