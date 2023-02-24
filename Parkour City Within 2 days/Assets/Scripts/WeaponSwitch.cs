using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int weaponSwitch = 0;
    public int weaponOpened = 0;
    public WeaponAiming weaponAiming;
    public Weapon weapon;
    public WeaponData[] weaponDatas;
    public GameObject[] weaponObjects;
    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int currentWeapon = weaponSwitch;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (weaponSwitch >= transform.childCount - weaponOpened)
            {
                weaponSwitch = 0;
            }
            else
            {
                weaponSwitch++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (weaponSwitch <= 0)
            {
                weaponSwitch = transform.childCount - weaponOpened;
            }
            else
            {
                weaponSwitch--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponSwitch = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponSwitch = 1;
        }

        /* if (Input.GetKeyDown(KeyCode.Alpha3))
         {
             weaponSwitch = 2;
         }

         if (Input.GetKeyDown(KeyCode.Alpha4))
         {
             weaponSwitch = 3;
         }
        */
        if (currentWeapon != weaponSwitch)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == weaponSwitch)
            {
                // weapon.reloading = false;
                weapon.ChangeGun(weaponDatas[i]);
                weapon.weaponObject = weaponObjects[i];
                GameObject selectedWeapon = transform.GetChild(i).gameObject;
                GameObject firePoint = selectedWeapon.transform.Find("FirePoint").gameObject;
                selectedWeapon.SetActive(true);
                weaponAiming.firePoint = firePoint;
                weapon.firePoint = firePoint.transform;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
