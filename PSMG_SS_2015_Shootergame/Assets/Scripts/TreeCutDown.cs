using UnityEngine;
using System.Collections;
using Assets.Scripts.Weapons;

public class TreeCutDown : MonoBehaviour {

    public int hitsToFall = 3;

    private int counter = 0;
    private Animation anim;

    private bool isFallen = false;

    private new EnviromentSound audio;

    private void onTomahawkAttack(BaseWeapon w)
    {        
        if (isFallen) return;
        
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (distance <= 5.5f)
        {
            counter++;

            Invoke("playHitSound", 1F);
            if (counter >= hitsToFall)
            {
                Invoke("timber", 1.5F);
            }
        }
    }

    private void playHitSound()
    {
        audio.playTreeHit();
    }

    private void timber()
    {
        audio.playTimber();
        anim.Play();
        isFallen = true;
    }

    private bool initialized = false;
    void FixedUpdate()
    {
        if (initialized) return;
        WeaponController wpc = player.GetComponent<WeaponController>();
        BaseWeapon tomahawk = wpc.getWeaponByName("Tomahawk");
        tomahawk.OnShotFired += new BaseWeapon.DOnWeaponInfoChanged(onTomahawkAttack);
        initialized = true;
    }

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        
        anim = GetComponent<Animation>();

        audio = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<EnviromentSound>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
