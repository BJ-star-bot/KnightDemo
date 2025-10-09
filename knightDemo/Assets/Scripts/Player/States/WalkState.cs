using TestGame.Movement;
using UnityEngine;

public class WalkState : IState
{
    private StateManager player;

    private Camera cam;
    public WalkState(StateManager p) { player = p; }

    public void enter()
    {
        cam = player.cam;
    }

    public void tick()
    {

        Vector3 cam_forward = cam.transform.forward;
        cam_forward.y = 0;
        cam_forward = cam_forward.normalized;
        Vector3 cam_right = Vector3.Cross(Vector3.up, cam_forward);
        float move_h = Input.GetAxis("Horizontal");
        float move_v = Input.GetAxis("Vertical");
        Vector3 move_dirction = (cam_forward * move_v + cam_right * move_h).normalized * player.walk_speed;

        if(Input.GetMouseButtonDown(0))player.change_state(new AttackState("Walk",player));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.change_state(new JumpState(player));
            return;
        }
        else if (move_dirction.magnitude < 0.1f)
        {
            player.change_state(new IdleState(player));
            return;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            player.change_state(new RunState(player));
            return;
        }

       Quaternion quater_rot = Quaternion.LookRotation(move_dirction.normalized, Vector3.up);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, quater_rot, 1f - Mathf.Exp(-player.turning_speed * Time.deltaTime));
        player.direction=move_dirction;//移动放在最后执行
        
    }

    public void exit() { }

    public AniInformation GetAniInf()
    {
        if (Input.GetMouseButton(1))
            return new AniInformation("Walk", "Block");//在walk时能进入防守
        
        else
            return new AniInformation("Walk");
}
}
