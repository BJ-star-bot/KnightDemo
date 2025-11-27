using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 强化版 LAN Host / Join 管理器：
/// - LocalHost → Loopback
/// - LAN Host → 绑定真实局域网 IPv4
/// - LAN Client → 连接 Host IPv4
/// </summary>
public class NetButton : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private ushort port = 7777;

    [Header("UI")]
    [SerializeField] private TMP_InputField lanInput;
    [SerializeField] private TMP_Text ipLabel;
    [SerializeField] private CanvasGroup selectionPanel;

    [Header("Buttons")]
    [SerializeField] private Button btnLocalHost;
    [SerializeField] private Button btnLocalClient;
    [SerializeField] private Button btnLanHost;
    [SerializeField] private Button btnLanJoin;
    [SerializeField] private Button btnRefreshIp;

    private UnityTransport transport;
    private bool waitingClient;

    private NetworkManager Net => networkManager != null ? networkManager : NetworkManager.Singleton;

    private void Awake()
    {
        if (networkManager == null)
            networkManager = NetworkManager.Singleton;

        ResolveTransport();

        btnLocalHost?.onClick.AddListener(StartLocalHost);
        btnLocalClient?.onClick.AddListener(StartLocalClient);
        btnLanHost?.onClick.AddListener(StartLanHost);
        btnLanJoin?.onClick.AddListener(StartLanJoin);
        btnRefreshIp?.onClick.AddListener(RefreshAndReset);

        RegisterCallbacks();
    }

    private void Start()
    {
        UpdateIpLabel();
    }

    // ------------------ Local Host (127.0.0.1) ------------------

    private void StartLocalHost()
    {
        EnsureTransport();
        transport.SetConnectionData("127.0.0.1", port);
        if (Net.StartHost())
            HidePanel();
    }

    private void StartLocalClient()
    {
        EnsureTransport();
        transport.SetConnectionData("127.0.0.1", port);
        waitingClient = Net.StartClient();
    }

    // ------------------ LAN Host (绑定真实局域网 IP) ------------------

    private void StartLanHost()
    {
        EnsureTransport();

        string lanIP = GetLocalIPv4();
        if (string.IsNullOrEmpty(lanIP))
        {
            Debug.LogError("无法获取局域网 IP，无法开启 Host!");
            return;
        }

        // 关键：绑定真实 LAN 网卡
        transport.SetConnectionData(lanIP, port);
        Debug.Log($"[LAN HOST] Listening on {lanIP}:{port}");

        if (Net.StartHost())
            HidePanel();
    }

    // ------------------ LAN Client ------------------

    private void StartLanJoin()
    {
        EnsureTransport();

        string address = lanInput?.text.Trim();
        if (string.IsNullOrEmpty(address))
        {
            Debug.LogWarning("LAN IP 为空，无法加入联机！");
            return;
        }

        transport.SetConnectionData(address, port);
        waitingClient = Net.StartClient();
    }

    // ------------------ Helpers ------------------

    private void RefreshAndReset()
    {
        UpdateIpLabel();
        ResetNetwork();
    }

    private void UpdateIpLabel()
    {
        if (ipLabel == null) return;

        string ip = GetLocalIPv4();
        ipLabel.text = string.IsNullOrEmpty(ip) ? "IP: N/A" : $"IP: {ip}";
    }

    private void ResetNetwork()
    {
        waitingClient = false;
        Net.Shutdown();
        ShowPanel();
    }

    private void HidePanel()
    {
        if (selectionPanel == null) return;

        selectionPanel.interactable = false;
        selectionPanel.blocksRaycasts = false;
        selectionPanel.alpha = 0;
    }

    private void ShowPanel()
    {
        if (selectionPanel == null) return;

        selectionPanel.interactable = true;
        selectionPanel.blocksRaycasts = true;
        selectionPanel.alpha = 1;
    }

    private void RegisterCallbacks()
    {
        if (Net == null) return;
        Net.OnClientConnectedCallback += OnClientConnected;
        Net.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientConnected(ulong id)
    {
        if (waitingClient && id == Net.LocalClientId)
        {
            waitingClient = false;
            HidePanel();
        }
    }

    private void OnClientDisconnected(ulong id)
    {
        if (id == Net.LocalClientId)
        {
            waitingClient = false;
            ShowPanel();
        }
    }

    private void EnsureTransport()
    {
        ResolveTransport();
        if (transport == null)
        {
            Debug.LogError("未找到 UnityTransport");
        }
    }

    private void ResolveTransport()
    {
        if (Net == null) return;
        transport = Net.NetworkConfig.NetworkTransport as UnityTransport;
        if (transport == null)
            transport = Net.GetComponent<UnityTransport>();
    }

    /// <summary>
    /// 获取真实的局域网 IPv4（剔除虚拟网卡、Loopback）
    /// </summary>
public static string GetLocalIPv4()
{
    var invalidKeywords = new string[]
    {
        "vpn", "virtual", "vmware", "hyper-v", "vbox", 
        "loopback", "tunnel", "tap", "tun", "zerotier", 
        "tailscale", "wg", "wireguard"
    };

    var interfaces = NetworkInterface.GetAllNetworkInterfaces()
        .Where(ni =>
            ni.OperationalStatus == OperationalStatus.Up &&
            ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
            ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
            !invalidKeywords.Any(k => 
                ni.Description.ToLower().Contains(k) || 
                ni.Name.ToLower().Contains(k))
        )
        // 优先 WiFi
        .OrderByDescending(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
        // 再按带宽排序
        .ThenByDescending(ni => ni.Speed);

    foreach (var ni in interfaces)
    {
        foreach (var ip in ni.GetIPProperties().UnicastAddresses)
        {
            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.Address.ToString();  // 找到第一个真实网卡 IPv4
            }
        }
    }

    return string.Empty;
}

}
