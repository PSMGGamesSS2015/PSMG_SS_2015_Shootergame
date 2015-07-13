
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
            reserveAmmo = 3;

            timeToReload = 5;
        }

        protected override bool Shoot()
        {
            bool success = base.Shoot();

            if (success) 
            {
                RaycastHit hit;
                if (Physics.Raycast(bulletSpawn.transform.position, Vector3.forward, out hit, 100.0f))
                {
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        hit.collider.gameObject.GetComponent<Enemy>().Health -= 999;
                    }
                }
            }

            return success;
        }

        protected override GameObject getViewmodelPrefab()
        {
            return null;
        }
    }
}
