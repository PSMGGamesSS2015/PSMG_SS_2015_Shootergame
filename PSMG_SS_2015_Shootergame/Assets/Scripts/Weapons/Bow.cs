// Script that handles the shooting of an arrow

using UnityEngine;
using System.Collections;
using Assets.Scripts;

namespace Assets.Scripts.Weapons
{
    public class Bow : BaseWeapon
    {

        // Minimum and maximum intensity that can be reached by the arrow
        private const float MIN_INTENSITY = 0.5f;
        private const float MAX_INTENSITY = 2.0f;

        // How fast the intensity should grow while bending the bow
        private const float INTENSITY_GROWTH = 1.0f;

        // Arrow prefab
        private GameObject arrowPrefab;

        // Current intensity, set automatically
        private float intensity;

        // Variable that determines if the player is bending the bow, set automatically
        private bool bending;

        public Bow(GameObject parent)
            : base("Bow", parent)
        {
            intensity = MIN_INTENSITY;

            PlayerPrefabsController ppc = parentPlayer.GetComponent<PlayerPrefabsController>();
            arrowPrefab = ppc.arrowPrefab;
        }

        protected override GameObject getViewmodelPrefab()
        {
            return parentPlayer.GetComponent<PlayerPrefabsController>().weaponBowPrefab;
        }

        protected override void SetSpecifications(ref int magazinSize, ref int reserveAmmo)
        {
            reserveAmmo = 20;
            magazinSize = 1;
        }

        public override void Update()
        {
            base.Update();

            // If the player is bending the bow (determined by CheckForButton()), increase the current intensity
            if (bending)
            {
                IncreaseIntensity();
            }
        }

        public override void FireButtonDown()
        {
            bending = true;
            intensity = 0;
            Animator.SetFloat("bendIntensity", intensity);
            Animator.SetTrigger("Shoot");
        }

        public override void FireButtonUp()
        {
            Shoot();
        }

        // Increases the current intensity
        void IncreaseIntensity()
        {
            // Increase the intensity
            intensity += INTENSITY_GROWTH * Time.deltaTime;

            Animator.SetFloat("bendIntensity", intensity);

            // Make sure the intensity does not exceed our limitations
            Mathf.Clamp(intensity, MIN_INTENSITY, MAX_INTENSITY);
        }

        protected override bool Shoot()
        {
            if (intensity < MIN_INTENSITY)
            {
                Animator.SetTrigger("DropArrow");
                return false;
            }

            if (!CanShoot())
            {
                return false;
            }

            bending = false;

            // Get the direction of the camera to set the arrow rotation like this
            Transform camPos = Camera.main.transform;
            

            // Instantiate a new arrow object and call its Shoot() function
            GameObject arrow = GameObject.Instantiate(arrowPrefab, bulletSpawn.transform.position, camPos.rotation) as GameObject;
            arrow.GetComponent<ArrowBehaviour>().Shoot(intensity);

            Animator.SetTrigger("ReleaseArrow");

            if (!base.Shoot())
            {
                return false;
            }

            return true;
        }
    }

}