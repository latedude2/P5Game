using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    private readonly float SHAKE_DEFORM_COEF = .2f;

    [SerializeField] private bool shake; //only for testing purposes

    [SerializeField] private bool isShaking;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeDecay;
    private float currentShakeIntensity;
    private Vector3 originalPos;
    private Quaternion originalRot;

    void Update()
    {
        TestHit();

        if (currentShakeIntensity > 0)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeIntensity;
            transform.rotation = new Quaternion(
               GetDeformedRotation(originalRot.x),
                GetDeformedRotation(originalRot.y),
                GetDeformedRotation(originalRot.z),
                GetDeformedRotation(originalRot.w)
            );

            currentShakeIntensity -= shakeDecay;
        }
        else if (isShaking)
        {
            isShaking = false;
        }
    }

    private float GetDeformedRotation(float axisValue)
    {
        return axisValue + Random.Range(-shakeIntensity, shakeIntensity) * SHAKE_DEFORM_COEF;
    }

    private void TestHit()
    {
        if (shake)
        {
            Hit();
            shake = false;
        }
    }

    // call this function from the follower's (bee swarm) script, when the distance is close enough
    public void Hit()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;

        currentShakeIntensity = shakeIntensity;
        isShaking = true;
    }
}
