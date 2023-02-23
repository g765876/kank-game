using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public RaycastHit2D hitInfo;
    public CharacterController2D characterController;
    public CameraShake cameraShake;

    public GameObject muzzleFlash;
    public float camShakeDuration= 5, camShakeMagnitude = 1;
    public Text text;

    private int damage;
    private float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    private int magazineSize, bulletsPetTap;
    private bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    private WeaponData lastGun;
    bool shooting, readyToShoot, reloading;
    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    public void ChangeGun(WeaponData data)
    {
        if(lastGun != null)
        {
            lastGun.bulletsLeft = bulletsLeft;
            lastGun.magazineSize = magazineSize;
        }
        damage = data.damage;
        timeBetweenShooting = data.timeBetweenShooting;
        spread = data.spread;
        range = data.range;
        reloadTime = data.reloadTime;
        timeBetweenShooting = data.timeBetweenShooting;
        magazineSize = data.magazineSize;
        bulletsPetTap = data.bulletsPetTap;
        allowButtonHold = data.allowButtonHold;
        bulletsLeft = data.bulletsLeft;
        lastGun = data;
    }
    void Update()
    {
        ShootSystem();
        text.text = $"{bulletsLeft} / {magazineSize}";
    }
    private void ShootSystem()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        if (Input.GetKey(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPetTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        float localSpread = spread;
        readyToShoot = false;
        //Spread
        if (characterController.m_wasCrouching)
        {
            localSpread = spread / 1.5f;
        }
        float x = Random.Range(-localSpread, localSpread);
        float y = Random.Range(-localSpread, localSpread);
        //Calculate direction
        Vector3 direction = firePoint.right + new Vector3(x, y, 0);
        //Raycast
        hitInfo = Physics2D.Raycast(firePoint.position, direction);
        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        lineRenderer.enabled = true;
        //Shake camera
        StartCoroutine(cameraShake.Shake(camShakeDuration, camShakeMagnitude));

        //Graphics
        Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);

        bulletsLeft--;
        bulletsShot--;
        
        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot > 0 && bulletsLeft > 0)
        {

        }
        //Invoke("Shoot", timeBetweenShooting);
    }
    private void ResetShot()
    {
        lineRenderer.enabled = false;
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
