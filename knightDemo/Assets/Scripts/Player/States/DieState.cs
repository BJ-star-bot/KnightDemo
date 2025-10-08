using UnityEngine;
using TestGame.Movement;

public class DieState : IState
{
    public StateManager Manager;
    public DieState(StateManager SM)
    {
        Manager = SM;
    }
    public void enter()
    {
        throw new System.NotImplementedException();
    }

    public void exit()
    {
        throw new System.NotImplementedException();
    }

    public AniInformation GetAniInf()
    {
        return new AniInformation("Die");
    }

    public void tick()
    {
        throw new System.NotImplementedException();
    }
}
