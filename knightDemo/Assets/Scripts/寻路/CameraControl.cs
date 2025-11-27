using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    public bool openCameraControl=false;

    public float lookSensitivity = 2f;     // 环视灵敏度
    public float panSpeed = 0.5f;          // 平移速度
    public float zoomSpeed = 10f;          // 推拉速度

    private float yaw;
    private float pitch;
    private bool rightMouseHeld = false;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))openCameraControl=!openCameraControl;
        if(!openCameraControl)return;
        // 右键按下时锁鼠标
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseHeld = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightMouseHeld = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 环视
        if (rightMouseHeld)
        {
            yaw += Input.GetAxis("Mouse X") * lookSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // 中键平移
        if (Input.GetMouseButton(2))
        {
            float dx = -Input.GetAxis("Mouse X") * panSpeed;
            float dy = -Input.GetAxis("Mouse Y") * panSpeed;
            transform.Translate(new Vector3(dx, dy, 0f), Space.Self);
        }

        // 滚轮推拉
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > Mathf.Epsilon)
        {
            transform.Translate(Vector3.forward * scroll * zoomSpeed, Space.Self);
        }
    }
}
