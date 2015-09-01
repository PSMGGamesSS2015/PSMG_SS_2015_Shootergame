/* BONUS TOTEM
 * Bonus totem is a special totem that can only be activated if normal totems has been activated.
 * Once activated, it is active for as long as all of the other totems are active. If one of the required totems expires, the bonus totem will be deactivated again and can only
 * be reactivated by first activating all of the other totems and then activating the bonus totem again.
 */

using UnityEngine;
using System.Collections;

public class BonusTotem : Totem {

    // Speed modifier that the player should receive after activating the bonus totem
    public float speedModifier = 1.5f;

    // Components
    private PlayerMovement movement;
    private BasePlayer basePlayer;

    new void Start()
    {
        // Find player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Get components
        basePlayer = player.GetComponent<BasePlayer>();
        movement = player.GetComponent<PlayerMovement>();
        base.Start();
    }

    // Upon activation of the totem...
    protected override void OnActivation()
    {
        // Set activatable to false
        activatable = false;

        // Increase the player's movement speed by the defined speed modifier
        movement.IncreaseSpeed(speedModifier);
        // Activate a bonus for the flaps while flying
        movement.ActivateFlapBonus();
        // Activate a bonus for sprinting
        basePlayer.ActivateSprintBonus();
    }

    // Upon deactivation of the totem...
    protected override void OnDeactivation()
    {
        // Reset all of the bonus effects
        movement.ResetSpeed();
        movement.DeactivateFlapBonus();
        basePlayer.DeactivateSprintBonus();
    }
    
}
