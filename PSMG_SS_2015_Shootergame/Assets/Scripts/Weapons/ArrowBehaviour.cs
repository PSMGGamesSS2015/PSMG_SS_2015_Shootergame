/* Every arrow is spawn with this script attached, so use this to control und manipulate every arrow object */

using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Weapons
{

    public class ArrowBehaviour : MonoBehaviour
    {

        // force: The force with which the arrow will be shot
        public float force = 500.0f;

        private bool wasFired = false;
        private bool isFlying = false;
        private bool wasPickedUp = false;
        private bool hitObject = false;

        // Sprite to show
        //public SpriteRenderer sprite;
		public GameObject arrowArrow;
		private int counter = 0;

        private new EnviromentSound audio;

        // Called once when the arrow spawns
        void Start()
        {
            //sprite.enabled = false;
			arrowArrow.SetActive (false);
        }

        void OnCollisionEnter(Collision collision)
        {
            isFlying = false;

            if (wasFired && !isFlying && !wasPickedUp)
            {
                if (collision.collider.gameObject.tag == "Player")
                {
                    WeaponController wp = collision.collider.gameObject.GetComponent<WeaponController>();
                    BaseWeapon bow = wp.getWeaponByName("Bow");
                    audio = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<EnviromentSound>();
                    if (bow != null)
                    {
                        audio.playCollected();
                        bow.ReserveAmmo++;
                        wasPickedUp = true;
                        wp.onCurWeaponInfoChanged(bow);
                    }
                    Destroy(gameObject);
                    
                }
                return;
            }

            GetComponent<Rigidbody>().isKinematic = true;

            transform.Translate(transform.forward, Space.Self);

            gameObject.transform.parent = collision.gameObject.transform;

            if (collision.collider.gameObject.tag == "Enemy")
            {
                Enemy enemy = collision.collider.gameObject.GetComponent<Enemy>();

                int damage = (int)Random.Range(90, 100);
                enemy.Health -= damage;
            }
            else
            {
				arrowArrow.SetActive(true);
                //sprite.enabled = true;
            }
            
            wasFired = true;
        }

        // Called once every frame
        void Update()
        {
			Debug.Log (arrowArrow.transform.localScale.x);
			if(counter < 30) {
				arrowArrow.transform.localScale += new Vector3(0.005f, 0.005f, 0.005f);
			} else {
				if(arrowArrow.transform.localScale.x <= 0.2f) {

				} else {
					arrowArrow.transform.localScale += new Vector3(-0.005f, -0.005f, -0.005f);
				}
			}
			if (counter > 60) {
				counter = 0;
			}
			counter++;
        }



        /* After instantiation, the arrow would just fall down at its spawnPoint - Shoot provides a function that can be called after an arrow was instantiated to actually shoot it.
         * Needs a float parameter intensity which is a multiplier for the force. Through intensity, the player can bend the bow for a longer time to shoot the arrow with a greater force */

        public void Shoot(float intensity)
        {
            // Get the Rigidbody component of the arrow and add a force to it in the direction that the arrow is currently facing
            GetComponent<Rigidbody>().AddForce(transform.forward * intensity * force);
            isFlying = true;
        }

        public bool HitObject()
        {
            return hitObject;
        }

        public void ObjectHit()
        {
            hitObject = true;
        }
    }
}