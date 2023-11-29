using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string name;
    public float cooldownTime;
    public float activeTime;
    
    public enum State
    {
        ready,
        active,
        cooldown
    }
    private State currentState = State.ready;
    
    public State GetState()
    {
        return currentState;
    }

    public void SetState( State newState)
    {
        currentState = newState;
    }

    public virtual void Activate(GameObject parent, GameObject camera) {}
    public virtual void Disable(GameObject parent, GameObject camera) {}
}
