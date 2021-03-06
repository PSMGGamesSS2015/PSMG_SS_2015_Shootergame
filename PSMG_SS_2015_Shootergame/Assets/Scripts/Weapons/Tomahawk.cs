﻿
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Weapons
{
    class Tomahawk : BaseWeapon
    {
        private WeaponSound wsound;

        public Tomahawk(GameObject parent)
            : base("Tomahawk", parent)
        {
            wsound = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<WeaponSound>();
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

        public override void SetDown()
        {
            Animator.SetTrigger("SetDown");
            base.SetDown();
        }

        private GameObject getHighestEnemyParent(GameObject o)
        {
            while (o != null)
            {
                o = o.transform.parent.gameObject;

                if (o.GetComponent<Enemy>() != null)
                {
                    return o;
                }
            }
            return null;
        }

        protected override bool Shoot()
        {
            bool success = base.Shoot();
            
            if (success)
            {
                Animator.SetTrigger("Attack");
                wsound.swingHawkSound();

                for (int i = 0; i < Enemy.enemies.Count; i++) {

                    GameObject curEnemy = Enemy.enemies[i];
                    // Is enemy close enough
                    float distance = Vector3.Distance(this.parentPlayer.transform.position, curEnemy.transform.position);
                    //Debug.LogError("Tomahawk: " + distance);
                    if (distance > 3.0f)
                    {
                        continue;
                    }

                    // Is enemy mostly centered in front of us
                    Vector3 direction = curEnemy.transform.position - this.parentPlayer.transform.position;

                    if (Vector3.Angle(this.parentPlayer.transform.forward, direction) < 40.0f)
                    {
                        Debug.LogError("Tomahawk: in angle");
                        // Check if something is blocking the hit
                        RaycastHit hit;
                        if (Physics.Raycast(this.parentPlayer.transform.position + Vector3.up, direction.normalized, out hit, 5.0f))
                        {
                            Debug.LogError("Tomahawk: in Raycast" + hit.collider.gameObject.ToString());

                            GameObject hitObject = getHighestEnemyParent(hit.collider.gameObject);
                            if (hitObject && hitObject == curEnemy)
                            {
                                Debug.LogError("Tomahawk: in correct enemy");
                                // Yeah slice 
                                curEnemy.GetComponent<Enemy>().Health -= 40;
                                wsound.playHitSound();
                            }
                        }
                    }

                }
                
            }
            return success;
        }
    }
}
