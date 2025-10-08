using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float FullHealth=100f;
    public float CurrentHealth;
    public StateManager Manager;
    void Start()
    {
        CurrentHealth = FullHealth;
    }
    public void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0f)
        {
            Manager.change_state(new DieState(Manager));
        }
    }
}
