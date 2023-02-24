using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponData", menuName = "WeaponDatas")]
public class WeaponData : ScriptableObject
{
    public int damage = 10;
    public int magazineSize = 7;
    public float timeBetweenShooting = 0.02f, spread = 0.1f, range = 200, reloadTime = 2f, timeBetweenShots = 0f;
    public int bulletsPetTap = 1;
    public bool allowButtonHold = false;
    public int bulletsLeft = 7, bulletsShot = 1;
    public float cameraShakeDuration = 0.3f;
    public float cameraShakeMagnitude = 0.3f;
}
