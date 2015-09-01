using UnityEngine;
using System.Collections;

public class BonusTotem : Totem {

    public float speedModifier = 1.5f;

    private PlayerMovement movement;
    private BasePlayer basePlayer;

    new void Start()
    {
        Debug.LogError("tralal");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        basePlayer = player.GetComponent<BasePlayer>();
        movement = player.GetComponent<PlayerMovement>();
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    protected override void OnActivation()
    {
        activatable = false;

        movement.IncreaseSpeed(speedModifier);
        movement.ActivateFlapBonus();
        basePlayer.ActivateSprintBonus();
    }

    protected override void OnDeactivation()
    {
        movement.ResetSpeed();
        movement.DeactivateFlapBonus();
        basePlayer.DeactivateSprintBonus();
    }
    
}
