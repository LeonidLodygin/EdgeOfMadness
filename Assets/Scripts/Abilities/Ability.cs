using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string name;
    public float cooldownTime;
    public float activeTime;

    public virtual void Activate(GameObject parent, GameObject camera) {}
    public virtual void Disable(GameObject parent, GameObject camera) {}
}
