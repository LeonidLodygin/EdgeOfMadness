using UnityEngine;

/// <summary>
/// Handling the player's use of abilities
/// </summary>
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    public Ability ability;
    private float cooldownTime = 10;
    private float activeTime = 10;
    
    void Update()
    {
        switch (ability.GetState())
        {
            case Ability.State.ready:
                if (PressCheck(ability.name))
                {
                    ability.SetState(Ability.State.active);
                    ability.Activate(gameObject, camera);
                    activeTime = ability.activeTime;
                } 
                break;
            case Ability.State.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    ability.Disable(gameObject, camera);
                    ability.SetState(Ability.State.cooldown);
                    cooldownTime = ability.cooldownTime;
                } 
                break;
            case Ability.State.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    ability.SetState(Ability.State.ready);
                } 
                break;
        }
    }

    /// <summary>
    /// Checking if the player has activated an ability
    /// </summary>
    /// <param name="name">Name of ability</param>
    /// <returns>True or false</returns>
    private bool PressCheck(string name)
    {
        return name switch
        {
            "Rage" => GetComponent<InputManager>().Rage,
            "Pulling" => GetComponent<InputManager>().Pulling,
            "Blink" => GetComponent<InputManager>().Blink,
            _ => false
        };
    }
}
