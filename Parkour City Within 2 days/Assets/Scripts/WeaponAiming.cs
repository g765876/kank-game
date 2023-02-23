using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class WeaponAiming : MonoBehaviour
{
    public Vector3 mousePos;
    public Vector3 targetPos;
    public CharacterController2D characterController;
    public GameObject firePoint;
    public GameObject fakeAim;
    public GameObject fakeMousePos;
    public float threshold;
    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        targetPos = (characterController.transform.position + mousePos) / 2f;
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + characterController.transform.position.x, threshold + characterController.transform.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + characterController.transform.position.y, threshold + characterController.transform.position.y);
        fakeMousePos.transform.position = targetPos;

        Vector3 rotation = mousePos - fakeAim.transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        fakeAim.transform.SetPositionAndRotation(characterController.transform.position, Quaternion.Euler(0f, 0f, rotZ));
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        firePoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        if (rotZ < 89 && rotZ > -89)
        {
            //weapon.GetComponent<SpriteRenderer>().flipY = false;
            if (characterController.m_FacingRight != true)
            {
                characterController.Flip();
            }
        }
        else
        {
            //weapon.GetComponent<SpriteRenderer>().flipY = true;
            if (characterController.m_FacingRight != false)
            {
                characterController.Flip();
            }
        }

    }
}
