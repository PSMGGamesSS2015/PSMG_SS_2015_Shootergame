
using UnityEngine;
namespace Assets.Scripts.Weapons
{
    class Tomahawk : BaseWeapon
    {
        public Tomahawk(GameObject parent)
            : base("Tomahawk", parent)
        {

        }

        protected override void SetSpecifications(ref int magazinSize, ref int reserveAmmo)
        {
            magazinSize = 1;
            reserveAmmo = INFINITE_AMMO;
            timeToReload = 1;
        }

        protected override GameObject getViewmodelPrefab()
        {
            return parentPlayer.GetComponent<PlayerPrefabsController>().weaponTomahawkPrefab;
        }
    }
}
