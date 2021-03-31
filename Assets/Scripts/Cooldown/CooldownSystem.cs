using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class organizes cooldowns in project if there will be any.
/// </summary>
public class CooldownSystem : MonoBehaviour
{
    private readonly List<CooldownData> cooldowns = new List<CooldownData>();
    private void Update() {
        ProcessCooldowns();
    }

    /// <summary>
    /// This function loops through all cooldowns every frame,
    /// and Removes the cooldown if it's decremented to zero.
    /// </summary>
    private void ProcessCooldowns()
    {
        float deltaTime = Time.deltaTime;

        for (int i = cooldowns.Count - 1; i >= 0 ; i--)
        {
            if(cooldowns[i].DecrementCooldown(deltaTime))
            {
                cooldowns.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// This function loops through cooldowns to check if cooldown is still active.
    /// </summary>
    /// <param name="id">cooldown ID</param>
    /// <returns>true if active</returns>
    public bool IsOnCooldown(int id)
    {
        foreach (CooldownData cooldown in cooldowns)
        {
            if(cooldown.Id == id) 
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// This function adds new cooldown to the cooldowns list.
    /// </summary>
    /// <param name="cooldown">New cooldown to add</param>
    public void AddCooldown(ICooldown cooldown)
    {
        cooldowns.Add(new CooldownData(cooldown));
    }

    /// <summary>
    /// This function is used to get remaining duration of cooldown.
    /// Loops through cooldowns if ID matches the parameter, 
    /// returns remaining time of that cooldown.
    /// </summary>
    /// <param name="id">ID of the targeted cooldown</param>
    /// <returns>Returns remaining time of cooldown, 0 if not matching any cooldown</returns>
    public float GetRemainingDuration(int id) 
    {
        foreach (CooldownData cooldown in cooldowns)
        {
            if(cooldown.Id != id) 
            {
                continue;           
            }

            return cooldown.RemainingTime;
        }

        return 0f;
    }

}

/// <summary>
/// This class is cooldown data class.
/// </summary>
public class CooldownData
{
    /// <summary>
    /// Cooldown Data constructor.
    /// Initializes the cooldown properties.
    /// </summary>
    /// <param name="cooldown">ICooldown class to init values for cooldown data</param>
    public CooldownData(ICooldown cooldown)
    {
        Id = cooldown.Id;
        RemainingTime = cooldown.CooldownDuration;
    }

    /// <summary>
    /// Cooldown ID Property
    /// </summary>
    /// <value>new int value</value>
    public int Id{get;}
    /// <summary>
    /// Cooldown time remaining Property.
    /// </summary>
    /// <value>new float value</value>
    public float RemainingTime {get; private set;}

    /// <summary>
    /// This function decrements the cooldown by passed parameter amount.
    /// </summary>
    /// <param name="deltaTime">Value to decrement from cooldown per frame</param>
    /// <returns>returns true if cooldown timer is decremented to 0</returns>
    public bool DecrementCooldown(float deltaTime)
    {
        RemainingTime = Mathf.Max(RemainingTime - deltaTime, 0f);
        return RemainingTime == 0f;
    }
}
