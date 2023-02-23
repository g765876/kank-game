using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : ScriptableObject
{
    public int damage = 7;
    public int magazineSize = 7;
    public float timeBetweenShooting = 0.2f, spread = 0.1f, range=200, reloadTime= 2f, timeBetweenShots = 0f;
    public int bulletsPetTap = 1;
    public bool allowButtonHold = false;
}
