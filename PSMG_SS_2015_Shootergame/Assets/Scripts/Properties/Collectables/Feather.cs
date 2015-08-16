using UnityEngine;
using System.Collections;

public class Feather : Collectable {

    protected override void OnCollect(Collider player)
    {
        BasePlayer component = player.GetComponent<BasePlayer>();

        if (component.getCurrentFlowers() < component.maxFlowers)
        {
            component.FlowerCollected();
        }   
    }
}
