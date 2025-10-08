using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public CharacterController cc;
    public Camera cam;
    [Header("Move Parameter")]
    public float walk_speed = 1.5f;
    public float run_speed = 4f;
    public float jump_force = 4f;
    public float jump_horizontal_speed = 2f;
    public float turning_speed = 7f;
    public float ReactSpeed = 2f;

    [HideInInspector]public IState current_state;
    [HideInInspector]public bool jump_up;
    [HideInInspector]public Vector3 direction;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        change_state(new IdleState(this));
    }


    void Update()
    {
        current_state?.tick();
        
    }

    public void change_state(IState state_to_change) {
        current_state?.exit();
        current_state = state_to_change;
        state_to_change.enter();
    }
}
