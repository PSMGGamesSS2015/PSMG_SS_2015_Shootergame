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
        public const int INFINITE_AMMO = -1;

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

        protected GameObject viewModel = null;

        private Animator viewModelAnimator = null;
        public Animator Animator
        {
            get
            {
                return viewModelAnimator;
            }
            set
            {
                viewModelAnimator = value;
            }
        }

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
            bulletSpawn.transform.position = Camera.main.transform.position;
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

            if (reserveAmmo < bulletsToReload && reserveAmmo != INFINITE_AMMO)
            {
                bulletsToReload = reserveAmmo;
            }

            // Move ammo from reserve to main slot
            if (reserveAmmo != INFINITE_AMMO)
            {
                reserveAmmo -= bulletsToReload;
            }    
            curAmmo += bulletsToReload;
        }

        public virtual void SetUp()
        {
            GameObject prefab = getViewmodelPrefab();

            if (prefab == null) return;

            viewModel = GameObject.Instantiate(prefab, parentPlayer.transform.position, parentPlayer.transform.rotation) as GameObject;
            viewModelAnimator = viewModel.GetComponent<Animator>();
            viewModel.transform.parent = parentPlayer.transform;
        }

        public virtual void SetDown()
        {
            if (viewModel != null)
            {
                GameObject.Destroy(viewModel);
            }
        }

        protected abstract GameObject getViewmodelPrefab();

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
            if (!CanShoot())
            {
                return false;
            }

            if (curAmmo == 0) return false;

            curAmmo--;

            OnShotFired(this);

            if (curAmmo == 0 && (reserveAmmo > 0 || reserveAmmo == INFINITE_AMMO))
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
