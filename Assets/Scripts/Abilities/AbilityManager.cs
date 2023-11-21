using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    public Ability ability;
    private float cooldownTime = 10;
    private float activeTime = 10;

    private enum State
    {
        ready,
        active,
        cooldown
    }

    private State state = State.ready;

    void Update()
    {
        switch (state)
        {
            case State.ready:
                if (GetComponent<InputManager>().Rage)
                {
                    ability.Activate(gameObject, camera);
                    state = State.active;
                    activeTime = ability.activeTime;
                } 
                break;
            case State.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    ability.Disable(gameObject, camera);
                    state = State.cooldown;
                    cooldownTime = ability.cooldownTime;
                } 
                break;
            case State.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = State.ready;
                } 
                break;
        }
    }
}
