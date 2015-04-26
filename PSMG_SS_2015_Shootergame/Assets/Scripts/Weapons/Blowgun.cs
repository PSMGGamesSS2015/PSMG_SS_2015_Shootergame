
using System;
using UnityEngine;
namespace Assets.Scripts.Weapons
{
    class Blowgun : BaseWeapon
    {

        public Blowgun(GameObject parent)
            : base("Blowgun", parent)
        {

        }

        protected override void SetSpecifications(ref int magazinSize, ref int reserveAmmo)
        {
            magazinSize = 1;
            reserveAmmo = 10;

            timeToReload = 1;
        }

        protected override bool Shoot()
        {
            throw new NotImplementedException();
        }
    }
}
