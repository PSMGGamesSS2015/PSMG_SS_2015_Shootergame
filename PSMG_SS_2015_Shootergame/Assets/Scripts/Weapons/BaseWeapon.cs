using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseWeapon
    {
        protected int magazinSize;
        protected int curAmmo;
        protected int reserveAmmo;

        // Seconds until reload is finished
        protected float timeToReload = 2;

        // Seconds until the next bullet can be fired (NOT IMPLEMENTED JET)
        protected float fireRate = 0;

        // Are we currently reloading?
        protected bool isReloading = false;

        // Reference to the player object
        protected GameObject parentPlayer;

        // Bullet spawn point
        protected GameObject bulletSpawn;

        // Timestamp when the last reload started
        private float startReloadTimestamp = 0;

        // name of weapon
        private string weaponName;
        public string Name
        {
            get
            {
                return weaponName;
            }
            set
            {
                weaponName = value;
            }
        }


        public BaseWeapon(string name, GameObject parent) {
            weaponName = name;
            parentPlayer = parent;

            SetStandardBulletSpawn();
            
        }

        private void SetStandardBulletSpawn()
        {
            bulletSpawn = new GameObject();
            bulletSpawn.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            bulletSpawn.transform.position += bulletSpawn.transform.forward * 0.78f;
            bulletSpawn.transform.parent = parentPlayer.transform;
        }



        public virtual void Reload()
        {
            // If no ammo is left, we don't have to reload
            if (reserveAmmo == 0 || isReloading)
            {
                return;
            }

            isReloading = true;
            startReloadTimestamp = Time.time;

            // Get max bullets we can possibly reload
            int bulletsToReload = magazinSize - curAmmo;

            if (reserveAmmo < bulletsToReload)
            {
                bulletsToReload = reserveAmmo;
            }

            // Move ammo from reserve to main slot
            reserveAmmo -= bulletsToReload;
            curAmmo += bulletsToReload;
        }

        private void CheckMouseButtons()
        {
            // Check if Fire button is down - only called once, so use a variable
            if (Input.GetButtonDown("Fire1"))
            {
                FireButtonDown();
            }

            // Once Fire button is released, change bending back to false and shoot
            if (Input.GetButtonUp("Fire1"))
            {
                FireButtonUp();
            }

            // ReloadButton starts reloading
            if (Input.GetKeyUp("r"))
            {
                Reload();
            }
        }

        public virtual void Update()
        {
            CheckMouseButtons();

            if (isReloading && Time.time - startReloadTimestamp >= timeToReload)
            {
                isReloading = false;
            }
        }

        public bool CanShoot()
        {
            return (curAmmo > 0 && !isReloading);
        }

        protected virtual bool Shoot()
        {
            if (isReloading)
            {
                return false;
            }

            if (curAmmo == 0) return false;

            curAmmo--;

            if (curAmmo == 0 && reserveAmmo > 0)
            {
                Reload();
                return false;
            }

            return true;
        }

        protected virtual void FireButtonDown()
        {
            Shoot();
        }

        protected virtual void FireButtonUp()
        {

        }
    }
}
