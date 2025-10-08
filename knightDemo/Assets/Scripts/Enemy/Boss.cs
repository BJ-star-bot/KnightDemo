using System;
using UnityEngine;
using TestGame.Combat;
public class Boss : BaseEnemy
{
    public string Name="x";
    public int boss_score = 3;
    public static Action<string> Ondied;
    
    protected override void DeadMethod()
    {
        base.DeadMethod();
        text_score.Instance.add_score(3);
        Ondied.Invoke(Name);
    }
    protected override void InjuredAction(DamageContext dct)
    {
        base.InjuredAction(dct);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player=other.GetComponent<CharacterMotor>();
            if (player != null)
            {
                player.SufferDamge(new DamageSource(Name, transform.position, Attack));
                
            }
        }
    }
}
