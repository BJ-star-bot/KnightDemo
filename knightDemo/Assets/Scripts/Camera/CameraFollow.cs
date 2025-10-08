using System;

using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public StateManager player;
    public Camera cam;

    float yaw= 0f;
    float pitch = 0f;

    [Header("offset")]
    
    public float offset_y = 2;
    public float offset_z = 3;
    public float max_pitch_angle = 80f;
    public float min_pitch_angle = -80f;

    [Header("灵敏度调整")]
    
    public float move_translation_smooth = 16f;//越高相机越跟手
    public float move_yaw_rotation_speed = 5f;//旋转速度
    public float move_pitch_rotation_speed = 5f;
    public float move_rotation_smooth = 10f;//旋转跟手程度

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void LateUpdate()
    {

        yaw += Input.GetAxis("Mouse X") * move_yaw_rotation_speed;
        yaw = Mathf.Repeat(yaw, 360);
        pitch -= Input.GetAxis("Mouse Y") * move_pitch_rotation_speed;
        pitch = Mathf.Clamp(pitch, min_pitch_angle,max_pitch_angle);
        //相机跟随平移+相机鼠标移动
        float lerp_parameter = 1f - Mathf.Exp(-move_translation_smooth * Time.deltaTime);//指数平滑
        Quaternion orbitrot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 mouse_cam_translation = player.transform.position + new Vector3(0, offset_y, 0) + orbitrot * new Vector3(0, 0, -offset_z);
        cam.transform.position = Vector3.Lerp(cam.transform.position, mouse_cam_translation, lerp_parameter);

        //相机旋转
        Quaternion desire_rot = Quaternion.LookRotation(player.transform.position + new Vector3(0, offset_y, 0) - mouse_cam_translation, Vector3.up);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, desire_rot, 1f - Mathf.Exp(-move_rotation_smooth * Time.deltaTime));
    }



}

