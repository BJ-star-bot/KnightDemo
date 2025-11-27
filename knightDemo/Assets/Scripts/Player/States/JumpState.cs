using TestGame.Movement;
using UnityEngine;

public class JumpState : IState
{
    private StateManager player;
    private Vector3 direction;
    private Vector3 intend_direction;
    private float jump_time = 0f;
   
    private Camera cam;
    public JumpState(StateManager p)
    {
        player = p;
        cam = player.cam;
    }
    public void enter()
    {
        direction = player.direction;
        jump_time = 0;
        player.jump_up = true;
    }
    public void tick()
    {
      
        Vector3 cam_forward = cam.transform.forward;
        cam_forward.y = 0;
        cam_forward = cam_forward.normalized;
        Vector3 cam_right = Vector3.Cross(Vector3.up, cam_forward);
        float move_h = Input.GetAxis("Horizontal");
        float move_v = Input.GetAxis("Vertical");
        intend_direction = (cam_forward * move_v + cam_right * move_h).normalized * player.jump_horizontal_speed;
        direction = Vector3.Lerp(direction, intend_direction, 1f - Mathf.Exp(-0.5f * Time.deltaTime));
        player.direction = direction;

        jump_time += Time.deltaTime;
        if(Input.GetMouseButtonDown(0))player.change_state(new AttackState("Jump",player));
        if(jump_time>0.2f && player.cc.isGrounded)player.change_state(new IdleState(player));
    }
    
    public void exit()
    {

    }
     public AniInformation GetAniInf()
    {
        return new AniInformation("Jump");
}
}
