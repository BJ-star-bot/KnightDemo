
using TestGame.Movement;
public interface IState
{
    public void enter();
    public void tick();
    public void exit();
    public AniInformation GetAniInf();

}
