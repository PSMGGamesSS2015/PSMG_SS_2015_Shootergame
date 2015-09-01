using UnityEngine;
using System.Collections;
using Assets.Scripts.Weapons;

[RequireComponent(typeof(Collider))]
public class Destroyable : MonoBehaviour {

    public int shotsNeeded = 3;
    public bool destroyAfterShot = false;
    public bool fallAfterShot = false;

	public float tomahawkAttackDistance = 5.5f;

	public bool allowTomahawkAttacks = true;
	public bool allowArrows = true;

	public float effectDelayTime = 1.0f;

    protected GameObject player;
    protected int health;
    protected float pct;

	// Use this for initialization
	protected void Start () {
        health = shotsNeeded;
        pct = 1.0f;
        player = GameObject.FindGameObjectWithTag("Player");

        if (fallAfterShot)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }

		if (allowTomahawkAttacks) {
			WeaponController wpc = player.GetComponent<WeaponController>();
			BaseWeapon tomahawk = wpc.getWeaponByName("Tomahawk");
			tomahawk.OnShotFired += new BaseWeapon.DOnWeaponInfoChanged(OnAttack);
		}
	}

	void OnAttack(BaseWeapon w) {
		float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
		
		if (distance <= tomahawkAttackDistance)
		{
			OnDamage ();
		}
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (allowArrows && other.gameObject.tag == "Arrow")
        {
            Assets.Scripts.Weapons.ArrowBehaviour component = other.GetComponent<Assets.Scripts.Weapons.ArrowBehaviour>();

            if (component.HitObject() == false)
            {
                component.ObjectHit();
                
				OnDamage();
            }
            
        }
    }

	void OnDamage() {
		health--;
		pct = (float)health / shotsNeeded;
		
		if (health == 0)
		{
			Invoke("CheckEffects", effectDelayTime);
			OnKill();
		}
	}

    void CheckEffects()
    {
        CheckFall();
        CheckDestroy();
    }

    void CheckFall()
    {
        if (fallAfterShot)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void CheckDestroy()
    {
        if (destroyAfterShot)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return pct;
    }

    protected virtual void OnKill()
    {

    }
}
