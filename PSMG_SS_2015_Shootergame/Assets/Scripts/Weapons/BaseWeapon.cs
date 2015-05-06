using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Weapons
{

    public abstract class BaseWeapon
    {
        protected int magazinSize = 0;
        protected int curAmmo = 0;
        public int CurAmmo
        {
            get { return curAmmo; }
            set { if (value > 0 && value <= magazinSize) curAmmo = value; }
        }
        protected int reserveAmmo = 0;
        public int ReserveAmmo
        {
            get { return reserveAmmo; }
            set { reserveAmmo = value; }
        }
        // Seconds until reload is finished
        protected float timeToReload = 2;

        // Seconds until the next bullet can be fired (NOT IMPLEMENTED JET)
        protected float fireRate = 0;

        // Are we currently reloading?
        protected bool isReloading = false;

        // Reference to the player object
        protected GameObject parentPlayer = null;

        // Bullet spawn point
        protected GameObject bulletSpawn = null;

        // Timestamp when the last reload started
        private float startReloadTimestamp = 0;

        // name of weapon
        private string weaponName = "";
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

        // Status delegates
        public delegate void Status(BaseWeapon weapon);
        private static void NullStatus(BaseWeapon w) { }
        public Status OnReloadStart = NullStatus;
        public Status OnReloadEnd = NullStatus;
        public Status OnShotFired = NullStatus;


        public BaseWeapon(string name, GameObject parent) {
            weaponName = name;
            parentPlayer = parent;
            SetSpecifications(ref magazinSize, ref reserveAmmo);
            curAmmo = magazinSize;
            SetStandardBulletSpawn();
            
        }

        protected abstract void SetSpecifications(ref int magazinSize, ref int reserveAmmo);

        private void SetStandardBulletSpawn()
        {
            bulletSpawn = new GameObject();
            bulletSpawn.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            bulletSpawn.transform.position += bulletSpawn.transform.forward * 1.2f;
            bulletSpawn.transform.parent = parentPlayer.transform;
        }



        public virtual void Reload()
        {
            // If no ammo is left, we don't have to reload
            if (reserveAmmo == 0 || isReloading)
            {
                return;
            }

            OnReloadStart(this);

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

        public virtual void Update()
        {

            if (isReloading && Time.time - startReloadTimestamp >= timeToReload)
            {
                isReloading = false;
                OnReloadEnd(this);
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

            OnShotFired(this);

            if (curAmmo == 0 && reserveAmmo > 0)
            {
                Reload();
                return false;
            }

            return true;
        }

        public virtual void FireButtonDown()
        {
            Shoot();
        }

        public virtual void FireButtonUp()
        {

        }
    }
}
