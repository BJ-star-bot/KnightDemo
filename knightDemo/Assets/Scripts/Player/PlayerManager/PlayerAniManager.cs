using TestGame.Movement;
using UnityEngine;

public class PlayerAniManager : MonoBehaviour
{
    public StateManager player;
    public Animator ani;
    private AnimatorStateInfo AttackAsi;

    void Update()
    {
        AttackAsi = ani.GetCurrentAnimatorStateInfo(1);//获取第二层Layer的动画信息
    }
    void LateUpdate()
    {

        ani.SetBool("Walk", false);
        ani.SetBool("Jump", false);
        ani.SetBool("Run", false);
        ani.SetBool("Attacked", false);
        AniInformation AniInf = player.current_state.GetAniInf();//自定义的结构，见NameSpace.cs
        switch (AniInf.AniName)//DefaultLayer
        {
            case "Idle": break;
            case "Walk":
                {
                    ani.SetBool("Walk", true);
                    break;
                }
            case "Run":
                {
                    ani.SetBool("Run", true);
                    break;
                }
            case "Jump":
                {
                    ani.SetBool("Jump", true);
                    break;
                }
            case "Attacked":
                {

                    ani.SetBool("Attacked", true);
                    ani.SetFloat("AttackedX", AniInf.MoveX);//根据受击方向做出不同方向动作
                    ani.SetFloat("AttackedY", AniInf.MoveY);
                    break;

                }
            case "SpecialIdle1":
                {
                    ani.SetTrigger("SpecialIdle1");
                    break;
                }
            case "SpecialIdle2":
                {
                    ani.SetTrigger("SpecialIdle2");
                    break;
                }
            case "SpecialIdle3":
                {
                    ani.SetTrigger("SpecialIdle3");
                    break;
                }
            case "Die":
                {
                    ani.SetTrigger("Die");
                    break;
                }
        }
        switch (AniInf.AttackLayer)//AttackLayer,控制角色上半身
        {
            case "StandAttack":
                {
                    ani.SetTrigger("StandAttack");
                    break;
                }
            case "360Attack":
                {
                    ani.SetTrigger("360Attack");
                    break;

                }
            case "JumpAttack":
                {
                    ani.SetTrigger("JumpAttack");
                    break;
                    
            }
        }
    }
}
