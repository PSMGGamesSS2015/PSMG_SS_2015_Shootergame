using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseWeapon
    {
        protected int magazinSize;
        protected int curAmmo;
        protected int reserveAmmo;

        protected float timeToReload = 0;

        protected float fireRate = 0;

        protected bool isReloading = false;

        protected GameObject parentPlayer;

        // Bullet spawn point
        protected Transform bulletSpawn;


        public BaseWeapon(GameObject parent) {
            parentPlayer = parent;

            SetStandardBulletSpawn();
            
        }

        private void SetStandardBulletSpawn()
        {
            bulletSpawn = GameObject.FindGameObjectWithTag("MainCamera").transform;
            bulletSpawn.position += bulletSpawn.forward * 0.78f;
        }



        public virtual void Reload()
        {
            // If no ammo is left, wie dont have to reload
            if (reserveAmmo == 0)
            {
                return;
            }

            // Get max bullets we can possibly reload
            int bulletsToReload = magazinSize;
            if (reserveAmmo < magazinSize)
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
        }

        public bool CanShoot()
        {
            return (curAmmo > 0);
        }

        protected virtual void Shoot()
        {
            if (isReloading)
            {
                return;
            }

            curAmmo--;

            if (curAmmo == 0)
            {
                Reload();
            }
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
