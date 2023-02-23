using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public RaycastHit2D hitInfo;
    public CharacterController2D characterController;
    public Camera camShake;

    public int damage = 20;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPetTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;
    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    void Update()
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
        readyToShoot = false;
        //Spread
        if (characterController.m_wasCrouching)
        {
            spread /= 1.5f;
        }
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
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

        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot > 0 && bulletsLeft > 0)
        {

        }
        Invoke("Shoot", timeBetweenShooting);
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
