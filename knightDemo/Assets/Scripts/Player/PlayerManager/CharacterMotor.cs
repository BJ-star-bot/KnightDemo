using UnityEngine;
using TestGame.Combat;

public class CharacterMotor : MonoBehaviour//角色的最终位移应用，其他地方只写速度
{
    public CharacterController cc;
    public StateManager player;
    public float gravity = -9.8f;
    private float yvelocity = 0f;
    private Vector3 direction;
    void Update()
    {
        direction = player.direction;
        if (player.jump_up)
        {
            player.jump_up = false;
            yvelocity = player.jump_force;
        }
        if (!cc.isGrounded)
        {
            yvelocity += gravity * Time.deltaTime;
        }
        else if(yvelocity<0f)yvelocity = -1f;
        cc.Move((Vector3.up * yvelocity+direction)*Time.deltaTime);
    }
    public void SufferDamge(DamageSource DS)
    {
        if (player.current_state.GetAniInf().AniName!="Attacked")
        player.change_state(new AttackedState(player, DS.Position));
        
    }
}
