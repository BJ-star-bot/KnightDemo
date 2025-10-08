using System.Collections.Generic;
using TestGame.Movement;
using UnityEngine;

public class IdleState : IState
{
    private StateManager player;
    private float SpecialMovementTimer=0f;
    private string Movement = "Idle";
    public IdleState(StateManager p)
    {
        player = p;
    }
    public void enter()
    {
        player.direction = Vector3.zero;
    }
    public void tick()
    {
        if(Input.GetMouseButtonDown(0))player.change_state(new AttackState("Idle",player));
        if (Input.GetAxis("Horizontal") != 0) player.change_state(new WalkState(player));
        if (Input.GetAxis("Vertical") != 0) player.change_state(new WalkState(player));
        if (Input.GetKeyDown(KeyCode.Space)) player.change_state(new JumpState(player));
        
        SpecialMovementTimer += Time.deltaTime;
        if (SpecialMovementTimer > 8f)
        {
            List<string> Specials=new List<string>
            {
                "SpecialIdle1","SpecialIdle2","SpecialIdle3"
            };

            Movement =Specials[Random.Range(0,3)];
            SpecialMovementTimer = 0f;

        }
        else Movement = "Idle";
    }
    public void exit()
    {

    }
    public AniInformation GetAniInf()
    {
        return new AniInformation(Movement);
}
}
