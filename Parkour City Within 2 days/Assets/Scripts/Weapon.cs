using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public GlobalEventManager GlobalEventManager;

    public Transform firePoint;
    public LineRenderer lineRenderer;
    public RaycastHit2D hitInfo;
    public CharacterController2D characterController;
    public CameraShake cameraShake;
    public MeshParticleSystem meshParticleSystem;

    public GameObject muzzleFlash;
    public GameObject bulletImpact;
    public GameObject bulletImpactGround;
    public GameObject weaponObject;
    private float camShakeDuration, camShakeMagnitude;
    public Text text;

    private int damage;
    private float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    private int magazineSize, bulletsPetTap;
    private bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    private WeaponData lastGun;
    bool shooting;
    bool readyToShoot = true;
    public bool reloading = false;
    private void Awake()
    {
        GlobalEventManager.OnShoot += GlobalEventManager_OnShoot;
    }

    private void GlobalEventManager_OnShoot(object sender, EventArgs e)
    {
        Shoot();
    }

    public void ChangeGun(WeaponData data)
    {
        if (lastGun != null)
        {
            lastGun.bulletsLeft = bulletsLeft;
            lastGun.magazineSize = magazineSize;
        }
        damage = data.damage;
        timeBetweenShooting = data.timeBetweenShooting;
        spread = data.spread;
        range = data.range;
        reloadTime = data.reloadTime;
        timeBetweenShots = data.timeBetweenShots;
        timeBetweenShooting = data.timeBetweenShooting;
        magazineSize = data.magazineSize;
        bulletsPetTap = data.bulletsPetTap;
        allowButtonHold = data.allowButtonHold;
        bulletsLeft = data.bulletsLeft;
        camShakeDuration = data.cameraShakeDuration;
        camShakeMagnitude = data.cameraShakeMagnitude;
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
           // Shoot();
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
        if (characterController.m_Rigidbody2D.velocity.magnitude > 3)
        {
            localSpread = spread * 1.5f;
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
                Instantiate(bulletImpact, hitInfo.point, firePoint.rotation);
            }
            else
            {
                Instantiate(bulletImpactGround, hitInfo.point, firePoint.rotation);
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
        cameraShake.Shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);

        meshParticleSystem.AddQuad(firePoint.position);


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
        weaponObject.GetComponent<Animator>().SetBool("isReloading", true);
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        weaponObject.GetComponent<Animator>().SetBool("isReloading", false);
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
