
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapons
{

    class WeaponController : MonoBehaviour
    {
        private BaseWeapon[] weapons;
        private const int MAX_WEAPONS = 4;
        private int numWeapons = 0;


        private int curWeaponIndex = 0;
        private int lastWeaponIndex = 0;

        public GameObject ammoInfoText = null;

        private BasePlayer player;

        public BaseWeapon.DOnWeaponInfoChanged onCurWeaponInfoChanged = BaseWeapon.NullStatus;

        public void Awake()
        {
            player = GetComponent<BasePlayer>();

            weapons = new BaseWeapon[MAX_WEAPONS];
            AddWeapon(new Bow(gameObject));
            AddWeapon(new Blowgun(gameObject));
            AddWeapon(new Tomahawk(gameObject));

            switchToWeaponById(curWeaponIndex);
        }

        public void Update()
        {
            getActiveWeapon().Update();
            CheckMouseButtons();
        }

        public void AddWeapon(BaseWeapon weapon)
        {
            if (numWeapons >= MAX_WEAPONS)
            {
                return;
            }
            weapon.OnShotFired += new BaseWeapon.DOnWeaponInfoChanged(onAmmoInfoChanged);
            weapon.OnReloadEnd += new BaseWeapon.DOnWeaponInfoChanged(onAmmoInfoChanged);
            weapons[numWeapons++] = weapon;
        }

        void onAmmoInfoChanged(BaseWeapon w) { onCurWeaponInfoChanged(w); }

        private int getWeaponIdByName(string name)
        {
            int id = -1;
            for (int i = 0; i < numWeapons; i++)
            {
                if (weapons[i].Name == name)
                {
                    id = i;
                }
            }
            return id;
        }

        public BaseWeapon getWeaponByName(string name)
        {
            int id = getWeaponIdByName(name);
            if (id != -1)
            {
                return weapons[id];
            }
            return null;
        }

        private void CheckMouseButtons()
        {
            if (player.FlyMode == true) return;

            // Check if Fire button is down
            if (Input.GetButtonDown("Fire1"))
            {
                getActiveWeapon().FireButtonDown();
            }

            // Once Fire button is released
            if (Input.GetButtonUp("Fire1"))
            {
                getActiveWeapon().FireButtonUp();
            }

            // ReloadButton starts reloading
            if (Input.GetKeyUp("r"))
            {
                getActiveWeapon().Reload();
            }

            if (Input.GetKeyUp("q"))
            {
                switchToLastWeapon();
            }

            for (int i = 0; i < MAX_WEAPONS; i++)
            {
                if (Input.GetKeyUp(KeyCode.Alpha1 + i))
                {
                    switchToWeaponById(i);
                }
            }
        }

        public BaseWeapon getActiveWeapon()
        {
            return weapons[curWeaponIndex];
        }

        public void switchToNextWeapon()
        {
            int newWeaponIndex = curWeaponIndex + 1;
            if (newWeaponIndex >= numWeapons)
            {
                newWeaponIndex = 0;
            }
            switchToWeaponById(newWeaponIndex);
        }

        public void switchToLastWeapon()
        {
            switchToWeaponById(lastWeaponIndex);
        }

        public void switchToWeaponById(int id)
        {
            lastWeaponIndex = curWeaponIndex;
            if (id < numWeapons && numWeapons > 0)
            {
                curWeaponIndex = id;
            }

            weapons[lastWeaponIndex].SetDown();
            weapons[curWeaponIndex].SetUp();

            onCurWeaponInfoChanged(getActiveWeapon());
        }
    }
}
