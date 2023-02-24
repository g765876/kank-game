using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cameraCinemachine;
    private float shakeCameraTime;
    private float shakeCameraTimeTotal;
    private float startingIntensity;
    public void Shake(float duration, float magnitude)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cameraCinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = magnitude;

        startingIntensity = magnitude;
        shakeCameraTime = duration;
        shakeCameraTimeTotal = duration;
    }
    private void Update()
    {
        if (shakeCameraTime > 0)
        {
            shakeCameraTime -= Time.deltaTime;


            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cameraCinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, 1 - (shakeCameraTime / shakeCameraTimeTotal));
        }
    }
}
