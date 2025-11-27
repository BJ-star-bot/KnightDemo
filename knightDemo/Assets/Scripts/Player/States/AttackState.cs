using UnityEngine;
using TestGame.Movement;
public class AttackState : IState
{
    float timer = 0f;
    private StateManager player;
    private string LastMovement;
    private bool fired;
    public AttackState(string LM, StateManager p)
    {
        LastMovement = LM;
        player = p;
    }
    public void enter()
    {
        timer = 0f;
        fired = false;
    }

    public void exit()
    {
        
    }

    public AniInformation GetAniInf()
    {
        if (!fired)
        {
            fired = true;//只在初始返回攻击动作，防止反复触发
            switch (LastMovement)
            {
                case "Idle":
                    {
                        return new AniInformation(LastMovement, "StandAttack");

                    }
                case "Walk":
                    {
                        return new AniInformation(LastMovement, "StandAttack");
                    }
                case "Run":
                    {
                        return new AniInformation(LastMovement, "360Attack");
                }
                case "Jump":
                    {
                        return new AniInformation(LastMovement, "JumpAttack");
                }
            }
            
            
        }
        return new AniInformation(LastMovement);//攻击的animator参数都是trigger，触发完一次就不用再触发了
}

    public void tick()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            if (Input.GetAxis("Horizontal") != 0) player.change_state(new WalkState(player));
            if (Input.GetAxis("Vertical") != 0) player.change_state(new WalkState(player));
        }
        if (timer > 0.7f)
        {
            player.change_state(new IdleState(player));
        }
        
    }
}
