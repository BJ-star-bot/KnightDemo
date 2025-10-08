using UnityEngine;
using TestGame.Movement;
using System.Data.Common;
public class AttackedState :  IState
{
    private StateManager player;
    Vector3 DamagePosition;
    float Timer;
    public AttackedState(StateManager P, Vector3 DP)
    {
        player = P;
        DamagePosition = DP;
    }
    public void enter()
    {
       player.direction = (player.transform.position - DamagePosition).normalized*player.ReactSpeed;
    }

    public void exit()
    {
       
    }


        public AniInformation GetAniInf()
    {
        Vector2 Position = new Vector2(DamagePosition.x - player.transform.position.x, DamagePosition.z - player.transform.position.z).normalized;
        return new AniInformation("Attacked",null,Position.x,Position.y);
}

    public void tick()
    {
        Timer += Time.deltaTime;
        if (Timer > 0.7f) player.change_state(new IdleState(player));
        
    }
}
