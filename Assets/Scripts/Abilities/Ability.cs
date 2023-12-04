using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstraction of the player's abilities
/// </summary>
public class Ability : ScriptableObject
{
    public string name;
    public float cooldownTime;
    public float activeTime;
    
    /// <summary>
    /// Each ability has three states: ready to use, active, on cooldown.
    /// </summary>
    public enum State
    {
        ready,
        active,
        cooldown
    }
    
    //By default, all abilities are ready to be used from the start
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
