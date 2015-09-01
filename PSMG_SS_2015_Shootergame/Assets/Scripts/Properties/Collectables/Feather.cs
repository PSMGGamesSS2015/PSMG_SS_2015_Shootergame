/* FEATHER
 * Feather is a collectable object that increases the player's amount of feathers by one upon collection of the object.
 */

using UnityEngine;
using System.Collections;

public class Feather : Collectable {

    protected override void OnCollect(Collider player)
    {
        // Get the BasePlayer component
        BasePlayer component = player.GetComponent<BasePlayer>();

        // Collect the feather!
        component.FeatherCollected(); 
    }
}
