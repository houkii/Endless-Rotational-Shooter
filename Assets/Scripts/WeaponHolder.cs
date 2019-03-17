using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform muzzle;

    // public Weapon ActiveWeapon;      // todo

    private Coroutine RecoilCoroutine = null;

    [SerializeField]
    private float RecoilPositionOffset = 0.16f;
    [SerializeField]
    private float RecoilRotationOffset = -5f;

    private Vector3 defaultLocalPosition;
    private Quaternion defaultLocalRotation;
    private Vector3 recoilPosition;
    private Quaternion recoilRotation;


    void Awake()
    {
        this.defaultLocalPosition = transform.localPosition;
        this.defaultLocalRotation = transform.localRotation;
        this.recoilPosition = defaultLocalPosition - new Vector3(0, 0, RecoilPositionOffset);
        this.recoilRotation = defaultLocalRotation * Quaternion.Euler(RecoilRotationOffset, 0, 0);
    }


    public void Shoot()
    {
        var ball = Instantiate(projectilePrefab, muzzle.position, muzzle.transform.rotation);

        // let only one recoil coroutine running
        if(RecoilCoroutine != null)
        {
            StopCoroutine(RecoilCoroutine);
        }
        RecoilCoroutine = StartCoroutine(Recoil(.05f, .15f));
    }

    // attackTime - rising, releasTime - falling (as in signal envelopes)
    // Could raise reticlePointer (ergo crosshair) aswell, but this way seems fun for me
    // Update: ...Actually couldn't do it, because accel/gyro controls pointer, silly me :) 
    private IEnumerator Recoil(float attackTime, float releaseTime)
    {
        float currentTime = 0f;
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        // time will be "slightly" exceeded (1 frame) to get positions right 
        while (transform.localPosition != recoilPosition)
        {
            float step = Mathf.Clamp01(currentTime / attackTime);
            transform.localPosition = Vector3.Lerp(startPosition, this.recoilPosition, step);
            transform.localRotation = Quaternion.Slerp(startRotation, this.recoilRotation, step);
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0f;

        // same as above
        while (transform.localPosition != startPosition)
        {
            float step = Mathf.Clamp01(currentTime / releaseTime);
            transform.localPosition = Vector3.Lerp(this.recoilPosition, this.defaultLocalPosition, step);
            transform.localRotation = Quaternion.Slerp(this.recoilRotation, this.defaultLocalRotation, step);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
