
using System.Collections.Generic;
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

        public override void SetDown()
        {
            Animator.SetTrigger("SetDown");
            base.SetDown();
        }

        protected override bool Shoot()
        {
            bool success = base.Shoot();
            
            if (success)
            {
                Animator.SetTrigger("Attack");

                for (int i = 0; i < Enemy.enemies.Count; i++) {

                    GameObject curEnemy = Enemy.enemies[i];
                    // Is enemy close enough
                    float distance = Vector3.Distance(this.parentPlayer.transform.position, curEnemy.transform.position);
                    Debug.LogError("Tomahawk: " + distance);
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
                        if (Physics.Raycast(this.parentPlayer.transform.position, direction.normalized, out hit, 5.0f))
                        {
                            Debug.LogError("Tomahawk: in Raycast" + hit.collider.gameObject.ToString());

                            if (hit.collider.gameObject == curEnemy)
                            {
                                Debug.LogError("Tomahawk: in correct enemy");
                                // Yeah slice 
                                curEnemy.GetComponent<Enemy>().Health -= 40;
                            }
                        }
                    }

                }
                
            }
            return success;
        }
    }
}
